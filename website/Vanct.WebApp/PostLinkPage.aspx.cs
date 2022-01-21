using System;
using System.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp
{
    public partial class PostLinkPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            lblTitle.Text = VanctContext.Translater.Translate(VanctContext.RequestName);
            viewer.PostlinkType = VanctContext.RequestName;
        }
    }
}