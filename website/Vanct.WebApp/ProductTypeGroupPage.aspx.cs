using System;
using System.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp
{
    public partial class ProductTypeGroupPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)return;
            productViewer.ProductTypeGroupId = VanctContext.RequestId;
            var group = VanctContext.ProductTypeGroupDao.Get(i => i.Id == VanctContext.RequestId);
            lblTitle.Text = group.Name;
        }
    }
}