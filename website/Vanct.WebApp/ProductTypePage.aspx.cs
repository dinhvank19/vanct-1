using System;
using System.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp
{
    public partial class ProductTypePage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            productViewer.ProductTypeId = VanctContext.RequestId;
            var group = VanctContext.ProductTypeDao.Get(i => i.Id == VanctContext.RequestId);
            lblTitle.Text = group.Name;
        }
    }
}