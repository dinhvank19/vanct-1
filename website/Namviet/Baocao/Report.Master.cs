using System;
using System.Web.UI;
using Namviet.Helpers;

namespace Namviet.Baocao
{
    public partial class Report : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionHelper.LoginUser == null)
            {
                Response.Redirect("~/Baocao/Login.aspx");
            }
        }
    }
}