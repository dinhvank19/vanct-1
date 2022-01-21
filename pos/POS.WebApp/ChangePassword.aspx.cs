using System;
using System.Web.UI;
using POS.WebApp.AppCode;

namespace POS.WebApp
{
    public partial class ChangePassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // get inprogress session
            var user = PosContext.User;

            // if existing session -> go to page OrderOverview
            if (user == null)
                Response.Redirect("~/Default.aspx");
        }

        protected void BtnChangePasswordClick(object sender, EventArgs e)
        {
            if (txtPassword.Text.Length == 0 || txtConfirmPassword.Text.Length == 0)
            {
                panel.Visible = true;
                lblMessage.Text = "Vui lòng nhập mật khẩu";
                return;
            }

            if (!txtConfirmPassword.Text.Equals(txtPassword.Text))
            {
                panel.Visible = true;
                lblMessage.Text = "Mật khẩu không hợp lệ. Vui lòng xác nhận lại mật khẩu";
                return;
            }

            // get inprogress session
            panel.Visible = true;
            lblMessage.Text = "Đổi mật khẩu thành công";
            PosContext.User.ChangedPassword(txtPassword.Text);
        }

        protected void BtnBackClick(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}