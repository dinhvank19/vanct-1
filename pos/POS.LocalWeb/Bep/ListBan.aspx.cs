using POS.LocalWeb.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POS.LocalWeb.Bep
{
    public partial class ListBan : System.Web.UI.Page
    {
        private readonly AceDbContext _db = new AceDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadData();
        }

        private void LoadData()
        {
            var tables = _db.GetTables().Where(i => i.IsBusy);
            gridTables.DataSource = tables;
            gridTables.DataBind();
        }
    }
}