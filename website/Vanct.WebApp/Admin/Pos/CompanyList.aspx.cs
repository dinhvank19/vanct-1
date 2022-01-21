using System;
using System.Globalization;
using System.Web.UI;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Pos
{
    public partial class CompanyList : Page
    {
        #region Page events

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
                //case "btnAddUser":
                //    var cid = e.CommandArgument.ToString().ToInt32();
                //    Response.Redirect("~/Admin/Pos/CompanyUserList.aspx?cid=" + cid);
                //    break;
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
            var list = VanctContext.PosCompanyDao.Gets();
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

            var record = new PosCompany
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                IsActive = ckIsActive.Checked,
                CreatedDate = DateTime.Now
            };

            try
            {
                record = VanctContext.PosCompanyDao.Insert(record);
                if (record == null)
                {
                    lblMessage.Text = "Lỗi hệ thống. Tạo thất bại.";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                return;
            }

            lblMessage.Text = "Tạo nhà hàng thành công";
            LoadData();
        }


        protected void Update(int id)
        {
            if (txtName.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập tên";
                return;
            }

            var record = VanctContext.PosCompanyDao.Get(i => i.Id == id);
            if (record == null)
            {
                lblMessage.Text = "Nhà hàng không tồn tại";
                return;
            }

            record.Id = txtRecordId.Value.ToInt32();
            record.Name = txtName.Text;
            record.Description = txtDescription.Text;
            record.IsActive = ckIsActive.Checked;

            try
            {
                lblMessage.Text = VanctContext.PosCompanyDao.Update(record)
                    ? "Cập nhật thành công."
                    : "Lỗi hệ thống. Tạo thất bại.";
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

            lblTitle.Text = "Thêm mới nhà hàng";
            txtDescription.Text = "";
            txtName.Text = "";
            lblMessage.Text = "";
            ckIsActive.Checked = true;
            txtRecordId.Value = "";

            btnReload.Visible = false;
            btnSave.Visible = false;
            btnSaveNew.Visible = true;
            btnReset.Visible = true;
        }

        protected void ReloadForm(int id)
        {
            CleanForm();
            lblTitle.Text = "Chỉnh sửa nhà hàng";
            var record = VanctContext.PosCompanyDao.Get(i => i.Id == id);
            if (record == null)
            {
                lblMessage.Text = "Nhà hàng không tồn tại";
                return;
            }

            txtRecordId.Value = record.Id.ToString(CultureInfo.InvariantCulture);
            txtName.Text = record.Name;
            txtDescription.Text = record.Description;
            ckIsActive.Checked = record.IsActive;
            lblMessage.Text = "";

            btnReload.Visible = true;
            btnSave.Visible = true;
            btnSaveNew.Visible = false;
            btnReset.Visible = false;
        }

        #endregion
    }
}