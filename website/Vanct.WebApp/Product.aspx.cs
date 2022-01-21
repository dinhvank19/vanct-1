using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp
{
    public partial class Product : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)return;
            var o = VanctContext.ProductDao.Get(i => i.Id == VanctContext.RequestId);
            var group = VanctContext.ProductTypeGroupDao.Get(i => i.Id == o.ProductTypeGroupId);
            var type = VanctContext.ProductTypeDao.Get(i => i.Id == o.ProductTypeId);
            lblTitle.Text = o.Name;
            image.ImageUrl = "~/UploadManage/ProductImages/" + o.ImageUrl;
            lblGroup.Text = string.Format("{0}, {1}",
                group.Name, type != null ? type.Name : string.Empty);
            lblPriceVnd.Text = o.PriceVnd;
            lblNote.Text = o.Note.Replace(Environment.NewLine, "<br />");
            lblWarranty.Text = o.Warranty;
            lblDescription.Text = o.Description;
            productViewer.ProductTypeGroupId = o.ProductTypeGroupId;
            productViewer.ProductTypeId = type != null ? type.Id : 0;
            productViewer.ExcludeProductId = o.Id;

            var lblHeader = (Literal)Page.Master.FindControl("lblHeader");
            lblHeader.Text =
                string.Format(
                    "<meta property=\"og:title\" content=\"{0}\" /><meta property=\"og:description\" content=\"{1}\" />",
                    lblTitle.Text,
                    o.Note);
        }
    }
}