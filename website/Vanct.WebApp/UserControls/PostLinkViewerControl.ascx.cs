using System;
using System.Linq;
using System.Web.UI;
using Vanct.Dal.Entities;

namespace Vanct.WebApp.UserControls
{
    public partial class PostLinkViewerControl : UserControl
    {
        public string PostlinkType { set; get; }
        public int ExcludePostlinkId { set; get; }
        public bool PostlinkHome { set; get; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadProducts();
        }

        protected void LoadProducts()
        {
            using (var db = new VanctEntities())
            {
                if (PostlinkHome)
                {
                    repeater.DataSource = (from i in db.PostLinks
                        where i.IsActive && i.IsHomeShowed
                              && i.PostLinkType.Equals(PostlinkType)
                              && (ExcludePostlinkId == 0 || i.Id != ExcludePostlinkId)
                        orderby i.Position descending
                        select new
                        {
                            i.Id,
                            i.Name,
                            i.ImageUrl,
                            Note = i.SmallOverviewContent
                        })
                        .ToList();
                }
                else
                {
                    repeater.DataSource = (from i in db.PostLinks
                        where i.IsActive
                              && i.PostLinkType.Equals(PostlinkType)
                              && (ExcludePostlinkId == 0 || i.Id != ExcludePostlinkId)
                        orderby i.Position descending
                        select new
                        {
                            i.Id,
                            i.Name,
                            i.ImageUrl,
                            Note = i.SmallOverviewContent
                        })
                        .ToList();
                }
                repeater.DataBind();
            }
        }
    }
}