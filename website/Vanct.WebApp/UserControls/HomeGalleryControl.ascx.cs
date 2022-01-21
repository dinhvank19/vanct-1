using System;
using System.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.UserControls
{
    public partial class HomeGalleryControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadHomeGallery();
        }

        public void LoadHomeGallery()
        {
            repeater.DataSource = VanctContext.HomeGalleryDao.Gets();
            repeater.DataBind();
        }
    }
}