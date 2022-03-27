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
            if (!tables.Any())
            {
                return;
            }

            List<ReportTableline> allLines = null;
            foreach (var table in tables)
            {
                var lines = table.Lines.Where(i => !i.DaChuyen).ToList();

                if (allLines == null)
                {
                    allLines = lines;
                }
                else
                {
                    allLines = allLines.Concat(lines).ToList();
                }
            }

            gridLines.DataSource = allLines.OrderBy(i => i.InDate).ToList();
            gridLines.DataBind();
        }

        protected void OnBtnDaDoc(object sender, EventArgs e)
        {
        }

        protected void OnBtnDaChuyen(object sender, EventArgs e)
        {
        }
    }
}