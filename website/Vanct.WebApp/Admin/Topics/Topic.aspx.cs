using System;
using System.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Topics
{
    public partial class Topic : Page
    {
        protected const string TopicImages = "~/UploadManage/TopicImages/";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            txtContent.ImageManager.ViewPaths = new[] { TopicImages };
            txtContent.ImageManager.UploadPaths = new[] { TopicImages };
            txtContent.ImageManager.DeletePaths = new[] { TopicImages };
            var o = VanctContext.TopicDao.Get(i => i.Id.Equals(VanctContext.RequestName));
            txtId.Text = o.Id;
            txtName.Text = o.Name;
            txtContent.Content = o.Content;
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            var o = VanctContext.TopicDao.Get(i => i.Id.Equals(VanctContext.RequestName));
            o.Name = txtName.Text;
            o.Content = txtContent.Content;
            VanctContext.TopicDao.Edit(o, i => i.Id.Equals(o.Id));
        }

        protected void BtnBackClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Topics/Topics.aspx", true);
        }
    }
}