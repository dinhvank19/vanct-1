using System;
using System.Linq;
using System.Web.UI;
using POS.LocalWeb.AppCode;
using POS.LocalWeb.Dal;
using POS.Shared;

namespace POS.LocalWeb.Biz
{
    public partial class ListProduct : Page
    {
        private readonly AceDbContext _db = new AceDbContext();
        public string CurrentColumnOption;
        public ReportTable CurrentTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            CheckTableStatus();
            LoadColumnOption();
            panelMessage.Visible = false;
            if (IsPostBack) return;
            LoadData();
            LoadGroupData();
            lblOrderType.Text = PosContext.IsRefund ? "Số lượng trả" : "Số lượng";
        }

        private void LoadColumnOption()
        {
            CurrentColumnOption = CacheContext.GetColumnOption();
        }

        private void LoadData()
        {
            var products = _db.GetProducts();
            if (!string.IsNullOrEmpty(txtProductGroupId.Value))
                products = products.Where(i => i.Group.Equals(txtProductGroupId.Value)).ToList();

            if (!string.IsNullOrEmpty(txtSearch.Text))
                products = products.Where(i => i.SearchName.Contains(txtSearch.Text.ToLower())).ToList();

            lblTableNo.Text = PosContext.RequestTableNo;

            gridProducts.DataSource = products;
            gridProducts.DataBind();
            OrderLinesCounter();
        }

        private void OrderLinesCounter()
        {
            CurrentTable = _db.GetTable(PosContext.RequestTableNo);
            if (CurrentTable.Lines.Count > 0)
            {
                lblOrderLinesCounter.Text = CurrentTable.Lines.Count.ToString();
            }
        }

        protected void BtnAddProduct(object sender, EventArgs e)
        {
            var user = PosContext.User;
            if (txtAmount.Text.Length == 0 || !txtAmount.Text.IsDouble())
                return;

            CurrentTable = _db.GetTable(PosContext.RequestTableNo);
            var amount = txtAmount.Text.ToDouble();
            var product = _db.GetProducts().Get(txtProductId.Value);
            if (PosContext.IsRefund)
            {
                amount *= -1;
                if (CurrentTable.UnableToRefund(product.Id, amount))
                {
                    panelMessage.Visible = true;
                    return;
                }
            }

            var line = new ReportOrderline
            {
                Dongia = product.Price,
                Dvt = product.Dvt,
                Mahg = product.Id,
                Tenhang = product.Name,
                Nhom = product.Muc,
                Maql = Guid.NewGuid().ToString().ToUpper(),
                Quay = user.Username,
                Manv = PosContext.User.Username.ToUpper(),
                Soban = PosContext.RequestTableNo,
                GhiChu = txtGhiChu.Text,
                SoLuong = amount
            };
            _db.InsertOrderline(line, PosContext.RequestTableNo);
            _db.BusyTable(PosContext.RequestTableNo);
            txtGhiChu.Text = string.Empty;
            txtAmount.Text = "";
            OrderLinesCounter();
        }

        protected void BtnSearch(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void CheckTableStatus()
        {
            CurrentTable = _db.GetTable(PosContext.RequestTableNo);
            if (CurrentTable.IsPrinted)
            {
                Response.Redirect("~/Biz/TableDetails.aspx?no=" + PosContext.RequestTableNo);
            }
        }

        protected void BtnBack(object sender, EventArgs e)
        {
            CurrentTable = _db.GetTable(PosContext.RequestTableNo);
            if (CurrentTable.Lines.Count == 0)
                Response.Redirect("~/Biz/ListTable.aspx?no=" + PosContext.RequestTableNo);
            else
                Response.Redirect("~/Biz/TableDetails.aspx?no=" + PosContext.RequestTableNo);
        }

        protected void ProductGroupChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadGroupData()
        {
            var data = _db.GetProductGroups();
            gridProductGroups.DataSource = data;
            gridProductGroups.DataBind();
        }
    }
}