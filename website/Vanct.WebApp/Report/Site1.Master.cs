using System;
using System.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Report
{
    public partial class Site1 : MasterPage
    {
        public string Username = "Báo cáo bán hàng";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (VanctContext.ReportUser == null) return;
            Username = VanctContext.ReportUser.Name;
        }
    }
}