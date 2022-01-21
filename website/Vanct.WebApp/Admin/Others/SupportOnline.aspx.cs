using System;
using System.Web.UI;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Others
{
    public partial class SupportOnline : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadSupportOnline();
        }

        protected void LoadSupportOnline()
        {
            grid.DataSource = VanctContext.SupportOnlineDao.Gets();
            grid.DataBind();
        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            string cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdDelete":
                    int id = e.CommandArgument.ToString().ToInt32();
                    VanctContext.SupportOnlineDao.Delete(i => i.Id == id);
                    LoadSupportOnline();
                    break;
            }
        }

        protected void BtnSaveClick(object sender, EventArgs e)
        {
            VanctContext.SupportOnlineDao.Create(new Dal.Entities.SupportOnline
                                               {
                                                   Skype = txtSkype.Text,
                                                   Facebook = txtFacebook.Text,
                                                   Hotline = txtHotline.Text,
                                                   Yahoo = txtYahoo.Text,
                                                   Email = txtEmail.Text
                                               });
            LoadSupportOnline();
        }
    }
}