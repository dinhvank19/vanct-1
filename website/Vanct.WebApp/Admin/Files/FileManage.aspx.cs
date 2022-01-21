using System;
using System.IO;
using System.Web.UI;
using Hulk.Shared;
using Telerik.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Files
{
    public partial class FileManage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            LoadData();
        }

        public void LoadData()
        {
            grid.DataSource = VanctContext.FileDao.Gets();
            grid.DataBind();
        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            string cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdDelete":
                    int id = e.CommandArgument.ToString().ToInt32();
                    BaseFile file = VanctContext.FileDao.Get(i => i.Id == id);
                    Path.Combine(AppPath.FileFolder, file.FilePath).DeleteFile();
                    VanctContext.FileDao.Delete(i => i.Id == id);
                    LoadData();
                    break;
                case "cmdEdit":
                    Response.Redirect(string.Format("FileUpdate.aspx?Id={0}", e.CommandArgument));
                    break;
            }
        }
    }
}