using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Web.UI;
using POS.LocalWeb.AppCode;
using POS.LocalWeb.Dal;
using POS.Shared;

namespace POS.LocalWeb.Biz
{
    public partial class TableDetails : Page
    {
        private readonly AceDbContext _db = new AceDbContext();
        public ReportTable CurrentTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentTable = _db.GetTable(PosContext.RequestTableNo);
            if (IsPostBack) return;
            HideButtonsIfPrinted();
            LoadData();
            LoadTables();
        }

        private void LoadTables()
        {
            ddlChangedToTableId.DataSource = _db.GetTables().Where(i => i.TableNo != PosContext.RequestTableNo);
            ddlChangedToTableId.DataTextField = "TableNo";
            ddlChangedToTableId.DataValueField = "TableNo";
            ddlChangedToTableId.DataBind();
        }

        protected void BtnTemporaryPrintOrder(object sender, EventArgs e)
        {
            var user = PosContext.User;
            var lines = CurrentTable.Lines.ToList();
            if (lines.Count == 0) return;
            
            var mayin = _db.GetProductExGroups().FirstOrDefault(i => i.IsTemporaryPrint);
            if (mayin == null) return;

            var content = "PHIẾU TẠM TÍNH" + Environment.NewLine;
            content += string.Format("Bàn {0} - {1}", CurrentTable.TableNo, user.Username) + Environment.NewLine;
            if (CurrentTable.InDate.HasValue)
                content += "Giờ vào: " + CurrentTable.InDate.Value.ToString("HH:mm") + Environment.NewLine;
            content += "Giờ ra: " + DateTime.Now.ToString("HH:mm") + Environment.NewLine;
            content += "-------------------------------------------" + Environment.NewLine;

            var groupByProductNames = lines.Select(i => i.ProductNo).Distinct().ToArray();
            var orderLines = groupByProductNames.Select(productNo => new ReportTableline
            {
                ProductName = lines.First(i => i.ProductNo == productNo).ProductName,
                Price = lines.First(i => i.ProductNo == productNo).Price,
                Amout = lines.Where(i => i.ProductNo == productNo).Sum(i => i.Amout)
            }).ToList();

            foreach (var line in orderLines)
            {
                content += string.Format("{0}{2}      {1} x {3} = {4}{5}",
                    line.ProductName,
                    line.PriceText,
                    Environment.NewLine,
                    line.Amout,
                    line.TotalText,
                    Environment.NewLine);

                if (!string.IsNullOrEmpty(line.GhiChu))
                    content += line.GhiChu + Environment.NewLine;
            }

            content += "-------------------------------------------" + Environment.NewLine;
            content += string.Format("Tổng cộng = {0}{1}", CurrentTable.TotalText, Environment.NewLine);

            // try to print
            if (!string.IsNullOrEmpty(mayin.Printer))
                PosContext.Print(content, mayin.Printer);

            Response.Redirect("~/Biz/ListTable.aspx");
        }

        protected void BtnPrintOrder(object sender, EventArgs e)
        {
            var user = PosContext.User;
            
            var lines = CurrentTable.Lines.Where(i => !i.IsPrinted).ToList();
            var groupIDs = lines.Select(i => i.ProductGroup).Distinct().ToArray();
            var mucs = _db.GetProductExGroups();
            mucs = mucs.Where(i => groupIDs.Contains(i.Name)).ToList();

            foreach (var g in mucs)
            {
                var content = "ORDER - " + g.Name + Environment.NewLine;
                content += string.Format("Bàn {0} - {1}", CurrentTable.TableNo, user.Username) + Environment.NewLine;
                content += DateTime.Now.ToString("yyyy-MM-dd HH:mm") + Environment.NewLine;
                content += "-------------------------------------------" + Environment.NewLine;
                content += "Món                                      SL" + Environment.NewLine;
                content += "-------------------------------------------" + Environment.NewLine;
                var glines = lines.Where(i => i.ProductGroup.Equals(g.Name)).ToList();
                if (glines.Count == 0)
                    continue;

                foreach (var line in glines)
                {
                    content += string.Format("{0} x {1}", line.ProductName, line.Amout) + Environment.NewLine;

                    if (!string.IsNullOrEmpty(line.GhiChu))
                        content += line.GhiChu + Environment.NewLine;

                    content += "-------------------------------------------" + Environment.NewLine;
                }

                // try to print
                if (!string.IsNullOrEmpty(g.Printer))
                {
                    PosContext.Print(content, g.Printer);
                    _db.UpdateOrderPrinted(glines.Select(i => string.Format("'{0}'", i.Id)).ToList());
                }
            }

            Response.Redirect("~/Biz/ListTable.aspx");
        }

        protected void HideButtonsIfPrinted()
        {
            if (CurrentTable.IsPrinted)
            {
                changeTablePanel.Visible = false;
                buttonsPanel.Visible = false;
            }
            else
            {
                if (PosContext.RequestChangeTable)
                {
                    changeTablePanel.Visible = true;
                    buttonsPanel.Visible = false;
                }
                else
                {
                    changeTablePanel.Visible = false;
                    buttonsPanel.Visible = true;
                }
            }
        }

        private void LoadData()
        {
            if (!CurrentTable.IsBusy && !CurrentTable.InDate.HasValue)
                Response.Redirect("~/Biz/ListProduct.aspx?no=" + PosContext.RequestTableNo);

            lblMoment.Text = CurrentTable.Moment;
            lblCheckIn.Text = CurrentTable.InDate?.ToString("yyyy-MM-dd HH:mm") ?? "";
            lblTableNo.Text = CurrentTable.TableNo;
            lblTotal.Text = CurrentTable.Total.ToMoney();
            gridLines.DataSource = CurrentTable.Lines;
            gridLines.DataBind();
        }

        protected void BtnDeleteProduct(object sender, EventArgs e)
        {
            var productId = txtDeleteOrderLineId.Value;
            _db.DeleteOrderline(productId);
            _db.ReleaseTable(PosContext.RequestTableNo);
            Response.Redirect("~/Biz/TableDetails.aspx?no=" + PosContext.RequestTableNo);
        }

        protected void BtnPerformChangeTable(object sender, EventArgs e)
        {
            var orderLineSelectedIDs = txtMoveToNewTableOrderLineSelectedIDs.Value;
            var newTableId = ddlChangedToTableId.SelectedValue;
            _db.MoveOrderLinesToNewTable(orderLineSelectedIDs, newTableId);
            _db.BusyTable(newTableId);
            _db.ReleaseTable(PosContext.RequestTableNo);
            Response.Redirect("~/Biz/ListTable.aspx");
        }
    }
}