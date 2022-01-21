using System;
using System.Globalization;
using System.Web.UI;
using POS.Dal;
using POS.Shared;
using Telerik.Web.UI;

namespace POS.WebApp.Admin
{
    public partial class AreaList : Page
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
            var list = RecordTableArea.All();
            grid.DataSource = list;
            grid.DataBind();
        }

        protected void Insert()
        {
            if (txtName.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập tên";
                return;
            }

            var record = new RecordTableArea
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                Discount = txtDiscount.Value != null ? (int) txtDiscount.Value.Value : 0,
                ValidStatus = cmbValidStatus.Value
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

            var record = RecordTableArea.Get(id);
            if (record == null)
            {
                lblMessage.Text = "Không có dữ liệu";
                return;
            }

            record.Name = txtName.Text;
            record.Description = txtDescription.Text;
            record.Discount = txtDiscount.Value != null ? (int) txtDiscount.Value.Value : 0;
            record.ValidStatus = cmbValidStatus.Value;

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
            txtDescription.Text = "";
            txtName.Text = "";
            lblMessage.Text = "";
            txtDiscount.Value = 0;
            cmbValidStatus.Index = 0;
            txtRecordId.Value = "";

            btnReload.Visible = false;
            btnSave.Visible = false;
            btnSaveNew.Visible = true;
            btnReset.Visible = true;
        }

        protected void ReloadForm(int id)
        {
            CleanForm();
            lblTitle.Text = "Chỉnh sửa";
            var record = RecordTableArea.Get(id);
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

            btnReload.Visible = true;
            btnSave.Visible = true;
            btnSaveNew.Visible = false;
            btnReset.Visible = false;
        }

        #endregion
    }
}