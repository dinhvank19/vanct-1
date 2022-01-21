using System;
using System.Linq;
using System.Web.UI;
using Hulk.Shared;
using Namviet.Data;
using Namviet.Helpers;

namespace Namviet.Baocao
{
    public partial class ChangePassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (SessionHelper.LoginUser.Username != "admin")
                Response.Redirect("~/Baocao/Default.aspx");
        }

        protected void RadButton1Click(object sender, EventArgs e)
        {
            using (var db = new NamvietEntities())
            {
                var username = SessionHelper.LoginUser.Username;
                var o = db.BaseUsers.Single(i => i.Username.Equals(username));
                o.Password = txtNewPass.Text.ToMd5();
                db.SaveChanges();
                lbl.Text = "thành công";
            }
        }

        protected void RadButton2Click(object sender, EventArgs e)
        {
            SessionHelper.LoginUser = null;
            Response.Redirect("~/Baocao/Login.aspx");
        }
    }
}