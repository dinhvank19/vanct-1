using System;
using System.Web.UI;
using Telerik.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp.Admin.Topics
{
    public partial class Topics : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack) return;
            LoadTopic();
        }

        protected void LoadTopic()
        {
            grid.DataSource = VanctContext.TopicDao.Gets();
            grid.DataBind();
        }

        protected void GridItemCommand(object sender, GridCommandEventArgs e)
        {
            string cmd = e.CommandName;
            switch (cmd)
            {
                case "cmdEdit":
                    Response.Redirect(string.Format("Topic.aspx?n={0}", e.CommandArgument));
                    break;
            }
        }
    }
}