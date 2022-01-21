using System;
using System.IO;
using System.Web.UI;
using Telerik.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Product
{
    public partial class ProductInsert : Page
    {
        protected const string TopicImages = "~/UploadManage/ProductImages/Editor";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            txtContent.ImageManager.ViewPaths = new[] { TopicImages };
            txtContent.ImageManager.UploadPaths = new[] { TopicImages };
            txtContent.ImageManager.DeletePaths = new[] { TopicImages };
            var position = VanctContext.ProductDao.Count(i => i.IsActive);
            txtPosition.Value = position + 1;
        }

        protected void BtnCreateClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Product/ProductInsert.aspx", true);
        }

        protected void BtnBackClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/Product/Products.aspx", true);
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            if (txtName.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập tên sản phẩm";
                return;
            }

            if (CmbProductGroup.ValueInt32 == 0)
            {
                lblMessage.Text = "Vui lòng chọn nhóm sản phẩm";
                return;
            }

            foreach (UploadedFile validFile in imageURL.UploadedFiles)
            {
                var newName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmfff"), validFile.GetExtension());
                var newImage = Path.Combine(AppPath.ProductImagesFolder, newName);
                validFile.SaveAs(newImage);

                var inserted = VanctContext.ProductDao.Create(new Dal.Entities.Product
                {
                    Name = txtName.Text,
                    Description = txtContent.Content,
                    ProductTypeGroupId = CmbProductGroup.ValueInt32,
                    IsActive = ckIsActive.Checked,
                    IsHomeShowed = ckIsHomeShowed.Checked,
                    PriceVnd = txtPriceVnd.Text,
                    ProductTypeId = CmbProductType.ValueInt32 == 0 ? (int?)null : CmbProductType.ValueInt32,
                    ImageUrl = Path.GetFileName(newImage),
                    Position = txtPosition.Value != null ? (int)txtPosition.Value : 0,
                    IsHot = ckIsHot.Checked,
                    IsSaleOff = ckIsSaleOff.Checked,
                    Warranty = txtWarranty.Text,
                    Note = txtNote.Text
                });

                if (inserted == null)
                {
                    lblMessage.Text = "không thể thêm sản phẩm mới";
                    return;
                }

                Response.Redirect("~/Admin/Product/ProductUpdate.aspx?Id=" + inserted.Id, true);
            }

            lblMessage.Text = "Vui lòng chọn hình đại diện sản phẩm";
        }
    }
}