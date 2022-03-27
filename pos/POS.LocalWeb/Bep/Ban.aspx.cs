using POS.LocalWeb.AppCode;
using POS.LocalWeb.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POS.LocalWeb.Bep
{
    public partial class Ban : System.Web.UI.Page
    {
        private readonly AceDbContext _db = new AceDbContext();
        public ReportTable CurrentTable;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadData();
        }

        protected void LoadData()
        {
            var tables = _db.GetTables().Where(i => i.IsBusy);
            if (!tables.Any()) return;

            List<ReportTableline> allLines = new List<ReportTableline>();
            foreach (var table in tables)
            {
                var lines = table.Lines.Where(i => !i.DaChuyen && i.ProductGroup.Equals("AN")).ToList();
                allLines = allLines.Concat(lines).ToList();
            }

            gridLines.DataSource = allLines.OrderBy(i => i.InDate).ToList();
            gridLines.DataBind();
        }

        protected void OnBtnDaDoc(object sender, EventArgs e)
        {
            var lineId = txtLineId.Value;
            _db.UpdateDaDoc(lineId);
            LoadData();
        }

        protected void OnBtnDaChuyen(object sender, EventArgs e)
        {
            var lineId = txtLineId.Value;
            _db.UpdateDaChuyen(lineId);
            PrintChuyen();
            Response.Redirect("~/Bep/Ban.aspx");
        }

        protected void PrintChuyen()
        {
            var lineId = txtLineId.Value;
            var line = _db.GetOrderLine(lineId);
            var content = $"**** {_db.BepTenPhieuChuyen()} *****{Environment.NewLine}{Environment.NewLine}" +
                $"Số bàn: {line.TableNo}{Environment.NewLine}" +
                $"{line.ProductName} - {line.Amout}{Environment.NewLine}" +
                $"Đã chuyển lúc {line.GioChuyen.GetValueOrDefault().ToString("HH:mm")}";
            var group = _db.GetProductExGroups().SingleOrDefault(i => i.Name == line.ProductGroup);
            if (group != null)
                PosContext.Print(content, group.Printer);
        }
    }
}