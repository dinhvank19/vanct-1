using System;
using System.Linq;
using System.Web.UI;
using POS.LocalWeb.Dal;

namespace POS.LocalWeb.Biz
{
    public partial class ListTable : Page
    {
        private readonly AceDbContext _db = new AceDbContext();
        public string CurrentColumnOption;

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadColumnOption();
            if (IsPostBack) return;
            LoadData();
        }

        private void LoadColumnOption()
        {
            CurrentColumnOption = CacheContext.GetColumnOption();
        }

        private void LoadData()
        {
            var tables = _db.GetTables();

            //var totalCurrent = tables.Sum(i => i.Total);
            //lblTotalCurrent.Text = totalCurrent.ToMoney();
            //lblTotalReceived.Text = _db.SumTotalReceived().ToMoney();

            var countTableBusy = tables.Count(i => i.IsBusy);
            var countTableInProgress = tables.Count(i => i.IsBusy && !i.Processed && i.Total > 0);
            lblCountTable.Text = tables.Count.ToString();
            lblCountBusy.Text = countTableBusy.ToString();
            lblTableInProgress.Text = countTableInProgress.ToString();

            gridTables.DataSource = tables;
            gridTables.DataBind();
        }

        protected void BtnRefresh(object sender, EventArgs e)
        {
            _db.Refresh();
            Response.Redirect("~/Biz/ListTable.aspx");
        }
    }
}