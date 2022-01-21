using System;
using System.Web.UI;
using Hulk.Shared;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin
{
    public partial class ChangePassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void RadButton1Click(object sender, EventArgs e)
        {
            VanctContext.BaseUserDao.ChangePassword(txtNewPass.Text.ToMd5());
            lbl.Text = "thành công";
        }

        protected void RadButton2Click(object sender, EventArgs e)
        {
            VanctContext.LoginUser = null;
            Response.Redirect("~/Admin/Login.aspx");
        }
    }
}