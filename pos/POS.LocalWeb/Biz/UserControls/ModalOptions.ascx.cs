using System;
using System.Web.Security;
using System.Web.UI;
using POS.LocalWeb.Dal;

namespace POS.LocalWeb.Biz.UserControls
{
    public partial class ModalOptions : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void BtnLogout(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("~/Default.aspx");
        }

        protected void BtnSaveColumnOption(object sender, EventArgs e)
        {
            CacheContext.Cacher.Set(CacheContext.ColumnOption, txtColumnOption.Value);
            Response.Redirect("~/Biz/ListTable.aspx");
        }
    }
}