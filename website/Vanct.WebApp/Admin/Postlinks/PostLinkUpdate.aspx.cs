using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Postlinks
{
    public partial class PostLinkUpdate : Page
    {
        protected const string TopicImages = "~/UploadManage/PostLinkImages/Editor";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            txtDescription.ImageManager.ViewPaths = new[] { TopicImages };
            txtDescription.ImageManager.UploadPaths = new[] { TopicImages };
            txtDescription.ImageManager.DeletePaths = new[] { TopicImages };
            LoadPostLink();
        }

        protected ImageFormat GetExtension(string extension)
        {
            extension = extension.ToLower();
            if (extension.Equals(".gif"))
                return ImageFormat.Gif;
            if (extension.Equals(".jpg"))
                return ImageFormat.Jpeg;
            if (extension.Equals(".png"))
                return ImageFormat.Png;
            return ImageFormat.Jpeg;
        }

        protected void LoadPostLink()
        {
            var id = VanctContext.RequestId;
            var postlink = VanctContext.PostLinkDao.Get(i => i.Id == id);
            txtDescription.Content = postlink.Description;
            txtLink.Text = postlink.Link;
            txtName.Text = postlink.Name;
            txtPosition.Value = postlink.Position;
            ckIsHomeShowed.Checked = postlink.IsHomeShowed;
            ckIsActive.Checked = postlink.IsActive;
            txtOverviewContent.Text = postlink.SmallOverviewContent;
            oldImage.ImageUrl = "~/UploadManage/PostLinkImages/" + postlink.ImageUrl;
            lblTitle.Text = VanctContext.Translater.Translate(postlink.PostLinkType);
            txtPostlinkType.Value = postlink.PostLinkType;
        }

        protected void BtnBackClicked(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Postlinks/PostLinks.aspx?n=" + txtPostlinkType.Value, true);
        }

        protected void BtnAddNewClicked(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Postlinks/PostLinkInsert.aspx?n=" + txtPostlinkType.Value, true);
        }

        protected void BtnSaveClicked(object sender, EventArgs e)
        {
            var id = VanctContext.RequestId;
            var postlink = VanctContext.PostLinkDao.Get(i => i.Id == id);
            postlink.Description = txtDescription.Content;
            postlink.Link = txtLink.Text;
            postlink.Name = txtName.Text;
            postlink.SmallOverviewContent = txtOverviewContent.Text;
            postlink.IsHomeShowed = ckIsHomeShowed.Checked;
            postlink.IsActive = ckIsActive.Checked;
            postlink.Position = txtPosition.Value != null ? (int) txtPosition.Value : 0;
            //postlink.PostLinkType = AppContext.RequestName;
            foreach (UploadedFile validFile in imageURL.UploadedFiles)
            {
                //delete old image
                Path.Combine(AppPath.PostLinkImagesFolder, postlink.ImageUrl).DeleteFile();
                string newName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmfff"), validFile.GetExtension());
                string newImage = Path.Combine(AppPath.PostLinkImagesFolder, newName);
                validFile.SaveAs(newImage);
                postlink.ImageUrl = Path.GetFileName(newImage);
            }
            VanctContext.PostLinkDao.Edit(postlink, i => i.Id == id);
            oldImage.ImageUrl = "~/UploadManage/PostLinkImages/" + postlink.ImageUrl;
            lblMessage.Text = "Đã chỉnh sửa " + VanctContext.Translater.Translate(postlink.PostLinkType) + " thành công";
        }
    }
}