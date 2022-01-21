using System;
using System.Net.Mail;
using System.Web.UI;
using MySetting = Hulk.Shared.Properties.Settings;

namespace Vanct.WebApp
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnSendClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCompany.Text))
            {
                lblMessage.Text = "Vui lòng nhập tên doanh nghiệp";
                return;
            }

            if (string.IsNullOrEmpty(txtContactName.Text))
            {
                lblMessage.Text = "Vui lòng nhập tên liên hệ";
                return;
            }

            if (string.IsNullOrEmpty(txtContactPhone.Text))
            {
                lblMessage.Text = "Vui lòng nhập số điện thoại";
                return;
            }

            if (string.IsNullOrEmpty(txtDescription.Content))
            {
                lblMessage.Text = "Vui lòng nhập nội dụng cần trao đổi";
                return;
            }

            //send email
            var smtp = new Hulk.Shared.Email.SmtpAccess(MySetting.Default.SmtpServer,
                MySetting.Default.SmtpEmailFrom,
                MySetting.Default.SmtpUsername,
                MySetting.Default.SmtpPassword,
                MySetting.Default.RequireSsl);

            var message = new MailMessage
            {
                From = new MailAddress(MySetting.Default.SmtpEmailFrom, MySetting.Default.DisplayName),
                IsBodyHtml = true,
                Body = string.Format(
                    "Tên doanh nghiệp: {0} <br/>Tên liên hệ: {1}<br/>Điện thoại: {2}<br/>Email: {3}<br/>Nội dung:{4}",
                    txtCompany.Text, txtContactName.Text, txtContactPhone.Text, txtContactEmail.Text,
                    txtDescription.Content),
                Subject = "ACE SOFT - Đăng ký"
            };

            message.To.Add(new MailAddress(Properties.Settings.Default.EmailForm));
            smtp.Send(message);
            lblMessage.Text = "Đã gửi, cám ởn đã sử dụng dịch vụ.";
        }

        protected void BtnResetClicked(object sender, EventArgs e)
        {
            Response.Redirect("~/Register.aspx");
        }
    }
}