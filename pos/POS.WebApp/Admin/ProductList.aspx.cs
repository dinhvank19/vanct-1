using System;
using System.Globalization;
using System.Web.UI;
using POS.Dal;
using POS.Shared;
using POS.WebApp.AppCode;
using Telerik.Web.UI;

namespace POS.WebApp.Admin
{
    public partial class ProductList : Page
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadData();
        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            var cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdInsert":
                    CleanForm();
                    break;
                case "cmdEdit":
                    var id = e.CommandArgument.ToString().ToInt32();
                    ReloadForm(id);
                    break;
            }
        }

        protected void BtnSaveNew(object sender, EventArgs e)
        {
            Insert();
        }

        protected void BtnReset(object sender, EventArgs e)
        {
            CleanForm();
            panCrud.Visible = false;
        }

        protected void BtnSave(object sender, EventArgs e)
        {
            var id = txtRecordId.Value.ToInt32();
            Update(id);
        }

        protected void BtnReload(object sender, EventArgs e)
        {
            var id = txtRecordId.Value.ToInt32();
            ReloadForm(id);
        }

        #endregion

        #region Insert | Update | Reload Form | Clean Form | Reset Password

        protected void LoadData()
        {
            var list = RecordProduct.All();
            grid.DataSource = list;
            grid.DataBind();
        }

        protected void Insert()
        {
            var user = PosContext.User;
            if (txtName.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập tên";
                return;
            }

            if (cmbProductGroup.ValueInt32 == 0)
            {
                lblMessage.Text = "Vui lòng chọn nhóm";
                return;
            }

            if (cmbProductOm.Value.Equals("None"))
            {
                lblMessage.Text = "Vui lòng chọn đơn vị tính";
                return;
            }

            var record = new RecordProduct
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                Discount = txtDiscount.Value != null ? (int) txtDiscount.Value.Value : 0,
                ValidStatus = cmbValidStatus.Value,
                CreatedBy = user.Id,
                GroupId = cmbProductGroup.ValueInt32,
                ProductOm = cmbProductOm.Value,
                Price = txtPrice.Value ?? 0
            };

            try
            {
                record.Insert();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                return;
            }

            lblMessage.Text = "Tạo thành công";
            LoadData();
        }


        protected void Update(int id)
        {
            if (txtName.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập tên";
                return;
            }

            if (cmbProductGroup.ValueInt32 == 0)
            {
                lblMessage.Text = "Vui lòng chọn nhóm";
                return;
            }

            if (cmbProductOm.Value.Equals("None"))
            {
                lblMessage.Text = "Vui lòng chọn đơn vị tính";
                return;
            }

            var record = RecordProduct.Get(id);
            if (record == null)
            {
                lblMessage.Text = "Không có dữ liệu";
                return;
            }

            var user = PosContext.User;
            record.Name = txtName.Text;
            record.Description = txtDescription.Text;
            record.Discount = txtDiscount.Value != null ? (int) txtDiscount.Value.Value : 0;
            record.ValidStatus = cmbValidStatus.Value;
            record.ChangedBy = user.Id;
            record.GroupId = cmbProductGroup.ValueInt32;
            record.ProductOm = cmbProductOm.Value;
            record.Price = txtPrice.Value ?? 0;

            try
            {
                record.Update();
                LoadData();
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void CleanForm()
        {
            panCrud.Visible = true;

            lblTitle.Text = "Thêm mới";
            lblMessage.Text = "";
            txtRecordId.Value = "";
            txtDescription.Text = "";
            txtName.Text = "";
            txtDiscount.Value = 0;
            cmbValidStatus.Index = 0;
            cmbProductGroup.Index = 0;
            cmbProductOm.Index = 0;
            txtPrice.Value = 0;

            btnReload.Visible = false;
            btnSave.Visible = false;
            btnSaveNew.Visible = true;
            btnReset.Visible = true;
        }

        protected void ReloadForm(int id)
        {
            CleanForm();
            lblTitle.Text = "Chỉnh sửa";
            var record = RecordProduct.Get(id);
            if (record == null)
            {
                lblMessage.Text = "Không có dư liệu";
                return;
            }

            txtRecordId.Value = record.Id.ToString(CultureInfo.InvariantCulture);
            txtName.Text = record.Name;
            txtDescription.Text = record.Description;
            txtDiscount.Value = record.Discount;
            cmbValidStatus.Value = record.ValidStatus;
            cmbProductGroup.ValueInt32 = record.GroupId;
            cmbProductOm.Value = record.ProductOm;
            txtPrice.Value = record.Price;

            btnReload.Visible = true;
            btnSave.Visible = true;
            btnSaveNew.Visible = false;
            btnReset.Visible = false;
        }

        #endregion
    }
}