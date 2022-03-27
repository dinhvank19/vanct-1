using System;
using POS.LocalWeb.AppCode;

namespace POS.LocalWeb.Biz
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PosContext.User.ChucVu.Equals("BEP"))
            {
                Response.Redirect("~/Bep/ListBan.aspx");
            }
        }
    }
}