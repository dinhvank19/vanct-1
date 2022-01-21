using System;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Report
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (VanctContext.ReportUser == null)
            {
                Response.Redirect("~/Report/Default.aspx");
            }
        }
    }
}