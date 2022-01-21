using System;
using System.Globalization;
using System.Net.Mail;
using System.Web.UI;
using Hulk.Shared;
using Hulk.Shared.Log;
using Hulk.Shared.Properties;
using Telerik.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Report
{
    public partial class ReportUser : Page
    {
        #region Page events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadData();
        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            string cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdInsert":
                    CleanForm();
                    break;
                case "cmdDelete":
                    var userId = e.CommandArgument.ToString().ToInt32();
                    DeleteUser(userId);
                    break;
                case "cmdEdit":
                    int id = e.CommandArgument.ToString().ToInt32();
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
            var id = txtUserId.Value.ToInt32();
            Update(id);
        }

        protected void BtnReload(object sender, EventArgs e)
        {
            var id = txtUserId.Value.ToInt32();
            ReloadForm(id);
        }

        protected void BtnResetPassword(object sender, EventArgs e)
        {
            var id = txtUserId.Value.ToInt32();
            ResetPassword(id);
        }

        #endregion

        #region Insert | Update | Reload Form | Clean Form | Reset Password | Delete User

        protected void DeleteUser(int userId)
        {
            using (var db = new VanctEntities())
            {
                var sql =
                    string.Format(
                        "delete w from ReportDailyline w inner join ReportDaily on w.DailyId = ReportDaily.Id where ReportDaily.RUserId = {0}; " +
                        "delete ReportDaily where RUserId = {0};" +
                        "delete RUser where Id = {0};", userId);
                db.Database.ExecuteSqlCommand(sql);
            }

            LoadData();
        }

        protected void ResetPassword(int id)
        {
            var password = StringUtil.RandomNumber(6);
            var user = VanctContext.ReportDao.Get(i => i.Id == id);
            if (user == null)
            {
                lblMessage.Text = "Tài khoản không tồn tại";
                return;
            }

            if (SendEmail(user, password))
            {
                user.Password = password.ToMd5();
                VanctContext.ReportDao.Edit(user, i => i.Id == id);
                lblMessage.Text = "Đổi mật khẩu thành công, vui lòng check mail.";
                LoadData();
                return;
            }
            lblMessage.Text = "Không gửi email được. Không thể đổi mật khẩu.";
        }

        protected void LoadData()
        {
            var list = VanctContext.ReportDao.Gets();
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

            if (txtPassword.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập mật khẩu";
                return;
            }

            if (txtAccessToken.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập KEY";
                return;
            }

            var password = txtPassword.Text;
            var accessToken = txtAccessToken.Text;
            var user = new RUser
            {
                Address = txtAddress.Text,
                ExpiredDate = txtExpiredDate.SelectedDate ?? DateTime.Now.AddYears(1),
                Name = txtName.Text,
                Username = txtName.Text.ClearVietKey(),
                IsActive = ckIsActive.Checked,
                Password = password,
                IsOnline = false,
                AccessToken = accessToken,
            };

            try
            {
                user = VanctContext.ReportDao.Insert(user);
                if (user == null)
                {
                    lblMessage.Text = "Lỗi hệ thống. Tạo tại khoản thất bại.";
                    return;
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                return;
            }

            lblMessage.Text = "Tạo tài khoản thành công";
            LoadData();
        }

        protected bool SendEmail(RUser user, string password)
        {
            try
            {
                //send email
                var smtp = new Hulk.Shared.Email.SmtpAccess(Settings.Default.SmtpServer,
                    Settings.Default.SmtpEmailFrom,
                    Settings.Default.SmtpUsername,
                    Settings.Default.SmtpPassword,
                    Settings.Default.RequireSsl);

                var message = new MailMessage
                {
                    From = new MailAddress(Settings.Default.SmtpEmailFrom, Settings.Default.DisplayName),
                    IsBodyHtml = true,
                    Body =
                        string.Format(
                            "Cập nhật tài khoản thành công!<br />Tài khoản: <b>{0}</b><br />Mật khẩu: <b>{1}</b><br />Tên: {3}<br />" +
                            "Địa chỉ: {4}<br />Ngày tạo: {2}<br />Ngày hết hạn: {5}<br />Key: <b>{6}</b>",
                            user.Username, password, DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"),
                            user.Name, user.Address, user.ExpiredDate.ToString("dd-MM-yyyy HH:mm:ss"),
                            user.AccessToken),
                    Subject = "ACE SOFT - Báo cáo trực tuyến - Tài khoản"
                };

                message.To.Add(new MailAddress(Properties.Settings.Default.EmailForm));
                smtp.Send(message);
                return true;
            }
            catch (Exception exception)
            {
                LoggingFactory.GetLogger().Log(string.Format("Send email failed !!! \n {0}", exception));
                return false;
            }
        }

        protected void Update(int id)
        {
            if (txtName.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập tên";
                return;
            }

            if (txtPassword.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập mật khẩu";
                return;
            }

            if (txtAccessToken.Text.Length == 0)
            {
                lblMessage.Text = "Vui lòng nhập KEY";
                return;
            }

            var user = VanctContext.ReportDao.Get(i => i.Id == id);
            if (user == null)
            {
                lblMessage.Text = "Tài khoản không tồn tại";
                return;
            }

            user.Id = txtUserId.Value.ToInt32();
            user.Address = txtAddress.Text;
            user.Name = txtName.Text;
            user.ExpiredDate = txtExpiredDate.SelectedDate ?? DateTime.Now.AddYears(1);
            user.IsActive = ckIsActive.Checked;
            user.AccessToken = txtAccessToken.Text;
            user.Password = txtPassword.Text;

            try
            {
                lblMessage.Text = VanctContext.ReportDao.Update(user)
                    ? "Cập nhật tài khoản thành công."
                    : "Lỗi hệ thống. Tạo tại khoản thất bại.";
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

            lblTitle.Text = "Thêm mới tài khoản";
            txtAddress.Text = "";
            txtExpiredDate.SelectedDate = DateTime.Now.AddYears(1);
            txtName.Text = "";
            txtUsername.Text = "";
            txtUserId.Value = "";
            lblMessage.Text = "";
            txtAccessToken.Text = "";
            txtPassword.Text = "";
            ckIsActive.Checked = true;

            btnReload.Visible = false;
            btnSave.Visible = false;
            btnResetPassword.Visible = false;
            btnSaveNew.Visible = true;
            btnReset.Visible = true;
        }

        protected void ReloadForm(int id)
        {
            CleanForm();
            lblTitle.Text = "Chỉnh sửa tài khoản";
            var user = VanctContext.ReportDao.Get(i => i.Id == id);
            if (user == null)
            {
                lblMessage.Text = "Tài khoản không tồn tại";
                return;
            }

            txtAddress.Text = user.Address;
            txtExpiredDate.SelectedDate = user.ExpiredDate;
            txtName.Text = user.Name;
            txtUsername.Text = user.Username;
            txtUserId.Value = user.Id.ToString(CultureInfo.InvariantCulture);
            ckIsActive.Checked = user.IsActive;
            txtAccessToken.Text = user.AccessToken;
            txtPassword.Text = user.Password;
            lblMessage.Text = "";

            btnReload.Visible = true;
            btnSave.Visible = true;
            btnResetPassword.Visible = false;
            btnSaveNew.Visible = false;
            btnReset.Visible = false;
        }

        #endregion

        
    }
}