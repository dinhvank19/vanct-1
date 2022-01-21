using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;
using Vanct.WebApp.UserControls;

namespace Vanct.WebApp
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)return;
            LoadDefaultPage();
        }

        protected void RepeaterItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if (item.ItemType != ListItemType.Item && item.ItemType != ListItemType.AlternatingItem) return;
            var data = (PostLinkType)item.DataItem;
            var postlinkView = (PostLinkViewerControl)item.FindControl("postlinkView");
            postlinkView.PostlinkType = data.Id;
        }

        protected void LoadDefaultPage()
        {
            repeater.DataSource = VanctContext.PostLinkDao.GetHomeTypes();
            repeater.DataBind();
        }
    }
}