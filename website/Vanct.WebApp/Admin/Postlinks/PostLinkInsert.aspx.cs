using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using Telerik.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Postlinks
{
    public partial class PostLinkInsert : Page
    {
        protected const string TopicImages = "~/UploadManage/PostLinkImages/Editor";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            txtDescription.ImageManager.ViewPaths = new[] { TopicImages };
            txtDescription.ImageManager.UploadPaths = new[] { TopicImages };
            txtDescription.ImageManager.DeletePaths = new[] { TopicImages };
            lblTitle.Text = VanctContext.Translater.Translate(VanctContext.RequestName);
            var position = VanctContext.PostLinkDao.Count(i => i.PostLinkType.Equals(VanctContext.RequestName));
            txtPosition.Value = position + 1;
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

        protected void BtnBackClicked(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Postlinks/PostLinks.aspx?n=" + VanctContext.RequestName, true);
        }

        protected void BtnAddNewClicked(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Postlinks/PostLinkInsert.aspx?n=" + VanctContext.RequestName, true);
        }

        protected void BtnSaveClicked(object sender, EventArgs e)
        {
            foreach (UploadedFile validFile in imageURL.UploadedFiles)
            {
                var newName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmfff"), validFile.GetExtension());
                var newImage = Path.Combine(AppPath.PostLinkImagesFolder, newName);
                validFile.SaveAs(newImage);
                VanctContext.PostLinkDao.Create(new PostLink
                                              {
                                                  Description = txtDescription.Content,
                                                  Link = txtLink.Text,
                                                  ImageUrl = Path.GetFileName(newImage),
                                                  Name = txtName.Text,
                                                  PostLinkType = VanctContext.RequestName,
                                                  SmallOverviewContent = txtOverviewContent.Text,
                                                  Position = txtPosition.Value != null ? (int)txtPosition.Value : 0,
                                                  IsActive = ckIsActive.Checked,
                                                  IsHomeShowed = ckIsHomeShowed.Checked,
                                              });
                lblMessage.Text = "Đã thêm " + VanctContext.Translater.Translate(VanctContext.RequestName) + " thành công";
            }
        }
    }
}