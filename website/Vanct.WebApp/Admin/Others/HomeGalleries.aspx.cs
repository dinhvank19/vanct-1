using System;
using System.Drawing.Imaging;
using System.IO;
using System.Web.UI;
using Vanct.Dal.Entities;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Others
{
    public partial class HomeGalleries : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadData();
        }

        public void LoadData()
        {
            grid.DataSource = VanctContext.HomeGalleryDao.Gets();
            grid.DataBind();
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

        protected void BtnSaveClicked(object sender, EventArgs e)
        {
            foreach (UploadedFile validFile in imageURL.UploadedFiles)
            {
                var newName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmfff"), validFile.GetExtension());
                var newImage = Path.Combine(AppPath.HomeGalleryFolder, newName);
                validFile.SaveAs(newImage);

                //string newThumbImage = Path.Combine(AppContext.ImageSlideThumbDirectory, newName);
                //var thumbBuffer = Image.FromFile(newImage);
                //thumbBuffer.ResizeImage(new Size(45, 45)).Save(newThumbImage, GetExtension(validFile.GetExtension()));

                VanctContext.HomeGalleryDao.Create(new HomeGallery
                {
                    Description = txtDescription.Text,
                    Link = txtLink.Text,
                    ImageUrl = Path.GetFileName(newImage),
                });

                LoadData();
            }
        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            string cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdDelete":
                    var id = e.CommandArgument.ToString().ToInt32();
                    var image = VanctContext.HomeGalleryDao.Get(i => i.Id == id);
                    Path.Combine(AppPath.HomeGalleryFolder, image.ImageUrl).DeleteFile();
                    VanctContext.HomeGalleryDao.Delete(i => i.Id == id);
                    LoadData();
                    break;
            }
        }
    }
}