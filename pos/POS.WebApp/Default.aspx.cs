using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using Newtonsoft.Json;
using POS.Dal;
using POS.Dal.Enums;
using POS.Shared;
using POS.WebApp.AppCode;
using POS.WebApp.Properties;

namespace POS.WebApp
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (Request.IsAuthenticated)
                Response.Redirect(
                        PosContext.User.UserType.Equals(UserType.Administrator.ToString())
                            ? "~/Admin/Default.aspx"
                            : "~/Mobile/Default.aspx", true);
        }

        protected void BtnLoginClick(object sender, EventArgs e)
        {
            try
            {
                var user = RecordUser.Login(txtUsername.Text, txtPassword.Text);
                var ticket = new FormsAuthenticationTicket(1, user.ToJson(),
                    DateTime.Now, DateTime.Now.AddMinutes(Settings.Default.AuthTimeout),
                    true, user.ToJson());
                var cookiestr = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookiestr)
                {
                    Expires = ticket.Expiration,
                    Path = FormsAuthentication.FormsCookiePath
                };
                Response.Cookies.Add(cookie);

                Response.Redirect(
                    user.UserType.Equals(UserType.Administrator.ToString())
                        ? "~/Admin/Default.aspx"
                        : "~/Mobile/Default.aspx", true);
            }
            catch (Exception exception)
            {
                lblMessage.Text = exception.Message.Translate();
                panel.Visible = true;
            }
        }
    }
}