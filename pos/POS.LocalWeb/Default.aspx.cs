using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using POS.LocalWeb.AppCode;
using POS.LocalWeb.Dal;
using POS.LocalWeb.Properties;
using POS.Shared;

namespace POS.LocalWeb
{
    public partial class Default : Page
    {
        private readonly AceDbContext _db = new AceDbContext();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogin(object sender, EventArgs e)
        {
            try
            {
                var user = _db.Login(txtUsername.Text, txtPassword.Text);
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

                if (user.ChucVu.Equals("TN"))
                {
                    Response.Redirect("~/Biz/ListTable.aspx");
                }
                else
                {
                    Response.Redirect("~/Biz/Bep.aspx");
                }
            }
            catch (Exception exception)
            {
                lblMessage.Text = exception.Message;
            }
        }
    }
}