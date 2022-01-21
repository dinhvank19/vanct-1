using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hulk.Shared;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Login1Authenticate(object sender, AuthenticateEventArgs e)
        {
            var user = VanctContext.BaseUserDao.Login(Login1.UserName, Login1.Password.ToMd5());
            if (user == null) return;
            VanctContext.LoginUser = user;
            Response.Redirect("~/Admin/Product/Products.aspx", true);
        }
    }
}