using System;
using System.Globalization;
using System.Web.UI;
using POS.Dal;
using POS.Shared;
using Telerik.Web.UI;

namespace POS.WebApp.Admin
{
    public partial class ProductGroupList : Page
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
            var list = RecordProductGroup.All();
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

            var record = new RecordProductGroup
            {
                Name = txtName.Text,
                ValidStatus = cmbValidStatus.Value,
                Description = txtDescription.Text,
                PrintersName = txtPrintersName.Text
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

            var record = RecordProductGroup.Get(id);
            if (record == null)
            {
                lblMessage.Text = "Không có dữ liệu";
                return;
            }

            record.Name = txtName.Text;
            record.ValidStatus = cmbValidStatus.Value;
            record.Description = txtDescription.Text;
            record.PrintersName = txtPrintersName.Text;

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
            txtName.Text = "";
            txtDescription.Text = "";
            txtPrintersName.Text = "";
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
            var record = RecordProductGroup.Get(id);
            if (record == null)
            {
                lblMessage.Text = "Không có dư liệu";
                return;
            }

            txtRecordId.Value = record.Id.ToString(CultureInfo.InvariantCulture);
            txtName.Text = record.Name;
            txtDescription.Text = record.Description;
            txtPrintersName.Text = record.PrintersName;
            cmbValidStatus.Value = record.ValidStatus;

            btnReload.Visible = true;
            btnSave.Visible = true;
            btnSaveNew.Visible = false;
            btnReset.Visible = false;
        }

        #endregion
    }
}