using POS.LocalWeb.AppCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace POS.LocalWeb.Bep
{
    public partial class BepLayout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (PosContext.User.ChucVu.Equals("TN"))
            {
                Response.Redirect("~/Biz/ListTable.aspx");
            }
        }
    }
}