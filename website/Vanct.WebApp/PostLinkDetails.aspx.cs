using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp
{
    public partial class PostLinkDetails : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            PostLink o = VanctContext.PostLinkDao.Get(i => i.Id == VanctContext.RequestId);
            viewer.PostlinkType = o.PostLinkType;
            viewer.PostlinkHome = false;
            viewer.ExcludePostlinkId = o.Id;

            lblTitle.Text = string.Format("{0} {1}", VanctContext.Translater.Translate(o.PostLinkType), o.Name);
            lblDescription.Text = o.Description;
            lblTitleSame.Text = VanctContext.Translater.Translate(o.PostLinkType);
            lblOverview.Text = o.SmallOverviewContent.Replace(Environment.NewLine, "<br />");

            var lblHeader = (Literal)Page.Master.FindControl("lblHeader");
            lblHeader.Text =
                string.Format(
                    "<meta property=\"og:title\" content=\"{0}\" /><meta property=\"og:description\" content=\"{1}\" />",
                    lblTitle.Text,
                    o.SmallOverviewContent);
        }
    }
}