using System.IO;
using System.Web.Hosting;

namespace Vanct.WebApp.AppCode
{
    public class AppPath
    {
        #region Path

        public static string ApplicationFolder
        {
            get { return HostingEnvironment.ApplicationPhysicalPath; }
        }

        public static string UploadManageFolder
        {
            get { return Path.Combine(ApplicationFolder, "UploadManage"); }
        }

        public static string HomeGalleryFolder
        {
            get { return Path.Combine(UploadManageFolder, "HomeGallery"); }
        }

        public static string PostLinkImagesFolder
        {
            get { return Path.Combine(UploadManageFolder, "PostLinkImages"); }
        }

        public static string ProductImagesFolder
        {
            get { return Path.Combine(UploadManageFolder, "ProductImages"); }
        }

        public static string FileFolder
        {
            get { return Path.Combine(UploadManageFolder, "BaseFileFolder"); }
        }

        public static string GoogleDrive
        {
            get { return Path.Combine(UploadManageFolder, "GoogleDrive"); }
        }


        #endregion
    }
}