using System;
using System.Linq;
using System.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp
{
    public partial class Download : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadPostlink();
        }

        protected void LoadPostlink()
        {
            repeater.DataSource = VanctContext.FileDao
                .Gets(i => !string.IsNullOrEmpty(i.FilePath))
                .OrderByDescending(i => i.Position)
                .ToList();
            repeater.DataBind();
        }
    }
}