using System;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Report
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            LoginControl.Visible = VanctContext.ReportUser == null;
            ReportPart.Visible = VanctContext.ReportUser != null;
        }
    }
}