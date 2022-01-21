using System;
using System.IO;
using System.Web.UI;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Product
{
    public partial class ProductUpdate : Page
    {
        protected const string TopicImages = "~/UploadManage/ProductImages/Editor";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            txtContent.ImageManager.ViewPaths = new[] { TopicImages };
            txtContent.ImageManager.UploadPaths = new[] { TopicImages };
            txtContent.ImageManager.DeletePaths = new[] { TopicImages };
            LoadProduct();
        }

        protected void LoadProduct()
        {
            var id = VanctContext.RequestId;
            var product = VanctContext.ProductDao.Get(i => i.Id == id);
            txtName.Text = product.Name;
            CmbProductGroup.ValueInt32 = product.ProductTypeGroupId;
            CmbProductType.TypeGroupId = product.ProductTypeGroupId;
            CmbProductType.Reload();
            CmbProductType.ValueInt32 = product.ProductTypeId ?? 0;
            txtPriceVnd.Text = product.PriceVnd;
            ckIsHomeShowed.Checked = product.IsHomeShowed;
            ckIsActive.Checked = product.IsActive;
            txtContent.Content = product.Description;
            ckIsHot.Checked = product.IsHot;
            ckIsSaleOff.Checked = product.IsSaleOff;
            txtNote.Text = product.Note;
            txtWarranty.Text = product.Warranty;
            txtPosition.Value = product.Position;
            oldImage.ImageUrl = "~/UploadManage/ProductImages/" + product.ImageUrl;
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

            var id = VanctContext.RequestId;
            var product = VanctContext.ProductDao.Get(i => i.Id == id);
            product.Name = txtName.Text;
            product.ProductTypeGroupId = CmbProductGroup.ValueInt32;
            product.ProductTypeId = CmbProductType.ValueInt32 == 0 ? (int?) null : CmbProductType.ValueInt32;
            product.PriceVnd = txtPriceVnd.Text;
            product.IsHomeShowed = ckIsHomeShowed.Checked;
            product.IsActive = ckIsActive.Checked;
            product.Description = txtContent.Content;
            product.Position = txtPosition.Value != null ? (int) txtPosition.Value : 0;
            product.IsHot = ckIsHot.Checked;
            product.IsSaleOff = ckIsSaleOff.Checked;
            product.Warranty = txtWarranty.Text;
            product.Note = txtNote.Text;
            foreach (UploadedFile validFile in imageURL.UploadedFiles)
            {
                var deleteImage = Path.Combine(AppPath.ProductImagesFolder, product.ImageUrl);
                deleteImage.DeleteFile();

                var newName = string.Format("{0}{1}", DateTime.Now.ToString("ddMMyyyyHHmmfff"), validFile.GetExtension());
                var newImage = Path.Combine(AppPath.ProductImagesFolder, newName);
                validFile.SaveAs(newImage);
                product.ImageUrl = Path.GetFileName(newImage);
            }
            VanctContext.ProductDao.Edit(product, i => i.Id == id);
            lblMessage.Text = "Chỉnh sửa sản phẩm thành công";
            LoadProduct();
        }
    }
}