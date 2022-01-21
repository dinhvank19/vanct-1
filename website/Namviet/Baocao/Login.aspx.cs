using System;
using System.Linq;
using Hulk.Shared;
using Namviet.Data;
using Namviet.Helpers;

namespace Namviet.Baocao
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLoginClick(object sender, EventArgs e)
        {
            using (var db = new NamvietEntities())
            {
                var password = txtPassword.Text.ToMd5();

                var user = db.BaseUsers.SingleOrDefault(i =>
                    i.Password.Equals(password) &&
                    i.Username.Equals(txtUsername.Text));

                panelError.Visible = user == null;
                if (user == null) return;
                SessionHelper.LoginUser = user;
                Response.Redirect("~/Baocao/Default.aspx", true);
            }
        }
    }
}