using System;
using System.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin
{
    public partial class AdminMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (VanctContext.LoginUser == null) Response.Redirect("~/Admin/Login.aspx", true);
        }
    }
}