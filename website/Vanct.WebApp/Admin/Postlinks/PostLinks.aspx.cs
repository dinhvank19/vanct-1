using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Postlinks
{
    public partial class PostLinks : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            lblTitle.Text = VanctContext.Translater.Translate(VanctContext.RequestName);
            LoadPostLink();
        }

        protected void LoadPostLink()
        {
            using (var db = new VanctEntities())
            {
                grid.DataSource = (from i in db.PostLinks
                    where i.PostLinkType.Equals(VanctContext.RequestName)
                    select new
                    {
                        i.Id,
                        i.Name,
                        HomeShowed = i.IsHomeShowed ? "Có" : "Không",
                        i.Position
                    }).ToList();
                grid.DataBind();
            }

        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            string cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdEdit":
                    Response.Redirect(string.Format("PostLinkUpdate.aspx?Id={0}", e.CommandArgument));
                    break;
                case "cmdDelete":
                    var id = e.CommandArgument.ToString().ToInt32();
                    var postlink = VanctContext.PostLinkDao.Get(i => i.Id == id);
                    Path.Combine(AppPath.PostLinkImagesFolder, postlink.ImageUrl).DeleteFile();
                    VanctContext.PostLinkDao.Delete(i => i.Id == id);
                    LoadPostLink();
                    break;
            }
        }
    }
}