using System;
using System.Linq;
using System.Web.UI;
using Vanct.Dal.BO;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.UserControls
{
    public partial class PostLinkPartnerControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadPostLink();
        }

        protected void LoadPostLink()
        {
            var customer = PostLinkType.Partner.ToString();
            repeater.DataSource = VanctContext.PostLinkDao
                .Gets(i => i.PostLinkType.Equals(customer))
                .OrderByDescending(i => i.Position)
                .ToList();
            repeater.DataBind();
        }
    }
}