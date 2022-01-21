using System;
using System.Web.Security;
using System.Web.UI;
using POS.Dal.Enums;
using POS.Shared;
using POS.WebApp.AppCode;

namespace POS.WebApp
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx", true);
        }
    }
}