using System;
using System.IO;
using System.Web.UI;
using Telerik.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Files
{
    public partial class FileInsert : Page
    {
        protected const string TopicImages = "~/UploadManage/BaseFileFolder/Editor";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            txtContent.ImageManager.ViewPaths = new[] { TopicImages };
            txtContent.ImageManager.UploadPaths = new[] { TopicImages };
            txtContent.ImageManager.DeletePaths = new[] { TopicImages };
            var position = VanctContext.FileDao.Count(i => i.Position >= 0);
            txtPosition.Value = position + 1;
        }

        protected void BtnCreateClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Files/FileInsert.aspx", true);
        }

        protected void BtnBackClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Files/FileManage.aspx", true);
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (txtName.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập tên sản phẩm";
                return;
            }

            string newFileName = string.Empty;
            foreach (UploadedFile validFile in fileUrl.UploadedFiles)
            {
                newFileName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmfff"),
                    validFile.GetExtension());
                string newFilePath = Path.Combine(AppPath.FileFolder, newFileName);
                validFile.SaveAs(newFilePath);
            }

            string newName = string.Empty;
            foreach (UploadedFile validFile in imageURL.UploadedFiles)
            {
                newName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmfff"), validFile.GetExtension());
                string newImage = Path.Combine(AppPath.FileFolder, newName);
                validFile.SaveAs(newImage);
            }

            var inserted = VanctContext.FileDao.Create(new BaseFile
            {
                Name = txtName.Text,
                Position = txtPosition.Value != null ? (int) txtPosition.Value : 0,
                FilePath = newFileName,
                ImageUrl = newName,
                HtmlContent = txtContent.Content
            });

            Response.Redirect("~/Admin/Files/FileUpdate.aspx?Id=" + inserted.Id, true);
        }
    }
}