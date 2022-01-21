using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using POS.Dal;
using POS.WebApp.AppCode;
using Telerik.Web.UI;

namespace POS.WebApp.Mobile
{
    public partial class OrderOverview : Page
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // get inprogress session
            var user = PosContext.User;

            // if existing session -> go to page OrderOverview
            if (user.Session == null)
                Response.Redirect("~/Default.aspx");

            GetAreas();
        }

        protected void BtnLogout(object sender, EventArgs e)
        {
            var user = PosContext.User;
            var tables = PosContext.BizOrder.GetTables();
            var clean = tables.Count(i => i.ActiveOrderId != null && i.Order.SessionId == user.Session.Id);
            if (clean == 0)
            {
                user.Session.Close();
                Response.Redirect("~/Logout.aspx");
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "myFunction", "myFunction(" + clean + ");", true);
            }
        }

        #endregion

        #region Tables

        private void GetAreas()
        {
            var data = PosContext.BizOrder.GetAreas();
            gridAreas.DataSource = data;
            gridAreas.DataBind();
        }

        private void GetTables(int areaId, RadListView grid)
        {
            var data = PosContext.BizOrder.GetTables()
                .Where(i => i.AreaId == areaId)
                .ToList();

            grid.DataSource = data;
            grid.DataBind();
        }

        protected void GridAreaItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            var item = e.Item;
            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
            {
                var group = (RecordTableArea)item.DataItem;
                var grid = (RadListView)item.FindControl("gridTables");
                GetTables(group.Id, grid);
            }
        }

        #endregion
    }
}