using System;
using System.IO;
using System.Web.UI;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Files
{
    public partial class FileUpdate : Page
    {
        protected const string TopicImages = "~/UploadManage/BaseFileFolder/Editor";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            txtContent.ImageManager.ViewPaths = new[] {TopicImages};
            txtContent.ImageManager.UploadPaths = new[] {TopicImages};
            txtContent.ImageManager.DeletePaths = new[] {TopicImages};
            LoadFile();
        }

        protected void LoadFile()
        {
            int id = VanctContext.RequestId;
            BaseFile file = VanctContext.FileDao.Get(i => i.Id == id);
            txtName.Text = file.Name;
            txtContent.Content = file.HtmlContent;
            txtPosition.Value = file.Position;
            if (!string.IsNullOrEmpty(file.ImageUrl))
                oldImage.ImageUrl = "~/UploadManage/BaseFileFolder/" + file.ImageUrl;

            if (!string.IsNullOrEmpty(file.FilePath))
            {
                oldFile.NavigateUrl = "~/UploadManage/BaseFileFolder/" + file.FilePath;
                oldFile.Text = ">Tại đây<";
            }
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
                lblMessage.Text = "Vui lòng nhập tên";
                return;
            }

            int id = VanctContext.RequestId;
            BaseFile file = VanctContext.FileDao.Get(i => i.Id == id);
            file.Name = txtName.Text;
            file.HtmlContent = txtContent.Content;
            file.Position = txtPosition.Value != null ? (int) txtPosition.Value : 0;

            foreach (UploadedFile validFile in imageURL.UploadedFiles)
            {
                if (!string.IsNullOrEmpty(file.ImageUrl))
                {
                    string deleteImage = Path.Combine(AppPath.FileFolder, file.ImageUrl);
                    deleteImage.DeleteFile();
                }
                

                string newName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmfff"),
                    validFile.GetExtension());
                string newImage = Path.Combine(AppPath.FileFolder, newName);
                validFile.SaveAs(newImage);
                file.ImageUrl = Path.GetFileName(newImage);
            }

            foreach (UploadedFile validFile in fileUrl.UploadedFiles)
            {
                if (!string.IsNullOrEmpty(file.FilePath))
                {
                    string deleteImage = Path.Combine(AppPath.FileFolder, file.FilePath);
                    deleteImage.DeleteFile();
                }

                string newName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmfff"),
                    validFile.GetExtension());
                string newImage = Path.Combine(AppPath.FileFolder, newName);
                validFile.SaveAs(newImage);
                file.FilePath = Path.GetFileName(newImage);
            }

            VanctContext.FileDao.Edit(file, i => i.Id == id);
            lblMessage.Text = "Chỉnh sửa thành công";
            LoadFile();
        }
    }
}