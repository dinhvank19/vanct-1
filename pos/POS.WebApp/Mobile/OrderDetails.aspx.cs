using System;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using POS.Dal;
using POS.Dal.Enums;
using POS.Shared;
using POS.WebApp.AppCode;
using Telerik.Web.UI;
using Image = System.Drawing.Image;

namespace POS.WebApp.Mobile
{
    public partial class OrderDetails : Page
    {
        #region Printer

        private void Printing(string filePath, string printerName)
        {
            using (var pd = new PrintDocument())
            {
                pd.PrinterSettings.PrinterName = printerName;
                pd.PrintPage += (sender, e) =>
                {
                    using (var img = Image.FromFile(filePath))
                        e.Graphics.DrawImage(img, new Point(10, 10));
                };
                pd.Print();
            }
        }

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // get inprogress session
            var user = PosContext.User;

            // if existing session -> go to page OrderOverview
            if (user.Session == null)
                Response.Redirect("~/Default.aspx");

            LoadProductGroups();
            LoadProducts();
            InitialOrder();
        }

        protected void ProductGroupChanged(object sender, EventArgs e)
        {
            var groupId = ddlProductGroup.SelectedValue.ToInt32();
            LoadProducts(groupId);
        }

        protected void BtnComplete(object sender, EventArgs e)
        {
            Table.Order.OrderStatus = OrderStatus.Completed.ToString();
            SaveOrder();
            Response.Redirect("~/Mobile/OrderOverview.aspx");
        }

        protected void BtnPrintBill(object sender, EventArgs e)
        {
            PrintBill();
        }

        protected void BtnPrintOrder(object sender, EventArgs e)
        {
            PrintOrder();
        }

        protected void BtnAddProductToOrder(object sender, EventArgs e)
        {
            AddProductToOrder();
        }

        protected void BtnBack(object sender, EventArgs e)
        {
            SaveOrder();
            Response.Redirect("~/Mobile/OrderOverview.aspx");
        }

        protected void BtnCancel(object sender, EventArgs e)
        {
        }

        #endregion

        #region Properties

        private bool _isLocked;

        private RecordTable _table;

        private RecordTable Table
        {
            get
            {
                if (_table != null) return _table;
                var tableId = PosContext.RequestId;
                if (PosContext.RequestId == 0)
                    Response.Redirect("~/Mobile/OrderOverview.aspx");
                _table = PosContext.BizOrder.GetTables().Get(tableId);
                return _table;
            }
        }

        #endregion

        #region Business Rules

        private void InitialOrder()
        {
            var user = PosContext.User;
            // if existing order
            if (Table.Order != null)
            {
                LoadOrder();
                return;
            }

            var order = new RecordOrder
            {
                CreatedDate = DateTime.Now,
                IsPrinted = false,
                SessionId = user.Session.Id,
                TableId = Table.Id,
                OrderStatus = OrderStatus.Pending.ToString(),
                OrderType = OrderType.Order.ToString(),
                StartTime = DateTime.Now
            };

            Table.Order = order;
            LoadOrder();
        }

        private void PrintBill()
        {
            if (Table.Order == null) return;
            if (Table.Order.Lines.Count == 0 && Table.Order.Id == 0)
            {
                Table.Order = null;
                return;
            }

            // get user
            var user = PosContext.User;

            // to to print process
            // convert bill to PNG
            var base64 = txtBillContentBase64.Value.Split(',')[1];
            var directory = Path.Combine(PosContext.UploadFolder, user.Session.Id.ToString());
            directory.CreateFolder();

            // write PNG on server
            var filePath = Path.Combine(directory, string.Format("{0}_{1}.png", Table.Order.Id, Table.Id));
            filePath.DeleteFile();
            base64.Base64ToImage(filePath);

            // print 
            Printing(filePath, "PRP-085 Printer");

            // update status and save order
            Table.Order.OrderStatus = OrderStatus.Printed.ToString();
            SaveOrder();
        }

        private void PrintOrder()
        {
            if (Table.Order == null) return;
            if (Table.Order.Lines.Count == 0 && Table.Order.Id == 0)
            {
                Table.Order = null;
                return;
            }

            // get all pending lines
            var pendingLines = Table.Order.Lines.Where(i => i.LineStatus.Equals(LineStatus.Pending.ToString())).ToList();
            foreach (var line in pendingLines)
                line.LineStatus = LineStatus.Printed.ToString();

            // save order
            SaveOrder();
        }

        private void SaveOrder()
        {
            if (_isLocked || Table.Order == null) return;
            if (Table.Order.Lines.Count == 0 && Table.Order.Id == 0)
            {
                Table.Order = null;
                return;
            }

            Table.Order.Save();

            if (Table.Order.OrderStatus.ToEnum<OrderStatus>() == OrderStatus.Completed)
            {
                Table.Order = null;
                Table.IsBusy = false;
                Table.ActiveOrderId = null;
                Table.Save();
            }
            else
            {
                Table.IsBusy = true;
                Table.ActiveOrderId = Table.Order.Id;
                Table.Save();
            }
        }

        private void LoadOrder()
        {
            // load bill content
            var data = Table.Order.Lines;
            gridBill.DataSource = data;
            gridBill.DataBind();

            var totalText = string.Format("{0}: {1}", Table.Name, Table.Order.TotalOrderText);
            lblTotal.Text = totalText;
            lblStartTime.Text = Table.Order.StartTimeText;

            // check locked
            _isLocked = PosContext.User.Session.Id != Table.Order.SessionId;
            txtOrderLocked.Value = _isLocked.ToString().ToLower();

            // load bill print content
            var printData = data.Select(i => new
            {
                ProductName = i.Product.Name,
                i.PriceText,
                i.TotalText,
                Amount = data.Where(j => j.ProductId == i.ProductId).Sum(j => j.Amount)
            }).Distinct();

            gridBillPrint.DataSource = printData;
            gridBillPrint.DataBind();
            lblBillTotal.Text = totalText;

            // load order-print content
            var groupIDs = Table.Order.Lines.Select(i => i.Product.GroupId).Distinct().ToList();
            var groups = PosContext.BizOrder.GetProductGroups().Where(i => groupIDs.Contains(i.Id)).ToList();
            gridProductGroup.DataSource = groups;
            gridProductGroup.DataBind();
        }

        protected void BilOrderItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                var group = (RecordProductGroup)item.DataItem;
                var grid = (RadListView)item.FindControl("gridBillPrint");
                var lblTableName = (Literal)item.FindControl("lblTableName");
                var lblDateTime = (Literal)item.FindControl("lblDateTime");

                lblTableName.Text = Table.Name;
                lblDateTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                grid.DataSource = Table.Order.Lines.Where(i => i.Product.GroupId == group.Id).ToList();
                grid.DataBind();
            }
        }

        private void AddProductToOrder()
        {
            if (_isLocked) return;

            var productId = txtProductId.Value.ToInt32();
            var amount = txtAmountLine.Value.ToInt32();
            var discount = txtDiscountLine.Value.ToInt32();

            var line = Table.Order.Lines.GetPendingLine(productId);
            if (line != null)
            {
                line.Discount = discount;
                line.Amount = line.Amount + amount;

                if (line.Amount <= 0)
                    Table.Order.Lines.Remove(productId);
            }
            else
            {
                var product = PosContext.BizOrder.GetProducts().Get(productId);
                Table.Order.Lines.Add(new RecordOrderline
                {
                    Amount = amount,
                    Price = product.Price,
                    CreatedDate = DateTime.Now,
                    Discount = discount,
                    LineStatus = LineStatus.Pending.ToString(),
                    ProductId = productId,
                    Product = product,
                    OrderId = Table.Order.Id
                });
            }

            LoadOrder();
        }

        #endregion

        #region Initial data

        private void LoadProducts(int groupId = 0)
        {
            var data = PosContext.BizOrder.GetProducts();
            if (groupId > 0)
                data = data.Where(i => i.GroupId == groupId).ToList();

            gridProducts.DataSource = data;
            gridProducts.DataBind();
        }

        private void LoadProductGroups()
        {
            var data = PosContext.BizOrder.GetProductGroups();
            ddlProductGroup.DataSource = data;
            ddlProductGroup.DataValueField = "Id";
            ddlProductGroup.DataTextField = "Name";
            ddlProductGroup.DataBind();
            ddlProductGroup.Items.Insert(0, new ListItem
            {
                Value = "0",
                Text = "Chọn nhóm"
            });
        }

        #endregion
    }
}