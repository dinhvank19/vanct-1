using System;
using System.Web.UI;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp
{
    public partial class TopicPage : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(IsPostBack)return;
            var topic = VanctContext.TopicDao.Get(i => i.Id.Equals(VanctContext.RequestName));
            lblTitle.Text = topic.Name;
            lblContent.Text = topic.Content;
        }
    }
}