using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using Vanct.Dal.Entities;
using Vanct.WebApp.AppCode;

namespace Vanct.WebApp
{
    public partial class Site1 : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            lblFooter.Text = VanctContext.TopicDao.Get(i => i.Id.Equals("footer")).Content;
            lblPageHeader.Text = VanctContext.TopicDao.Get(i => i.Id.Equals("header")).Content;
            //LoadPartner();
            //lblFooter.Text = AppContext.TopicDao.Get(i => i.Id.Equals("footer")).Content;
            //var supportOnline = AppContext.SupportOnlineDao.Gets().FirstOrDefault();
            //if (supportOnline == null) return;
            //const string s = "<a href=\"skype:{0}?chat\"><div class=\"skype left\"></div></a>" +
            //                 "<a href=\"ymsgr:sendim?{1}\"><div class=\"yahoo left\"></div></a>" +
            //                 "<a href=\"mailto:{3}\"><div class=\"email left\"></div></a>" +
            //                 "<div class=\"hotline left\">{2}</div>" +
            //                 "<div class=\"clr\"></div>";
            //lblSupportOnline.Text = string.Format(s, supportOnline.Skype, supportOnline.Yahoo, supportOnline.Hotline, supportOnline.Facebook);
        }

        public void LoadPartner()
        {
            //var partner = PostLinkType.Customer.ToString();
            //partnerRepeater.DataSource = AppContext.PostLinkDao.Gets(i => i.PostLinkType.Equals(partner));
            //partnerRepeater.DataBind();
        }

        protected void PartnerItemDataBound(object sender, RadRotatorEventArgs e)
        {
            var item = e.Item;
            var data = (PostLink) item.DataItem;
            var link = (HyperLink) item.FindControl("link");
            link.NavigateUrl = data.Link;
            link.Target = "_bank";
            var image = (Image) item.FindControl("img");
            image.ImageUrl = string.Format("~/UploadManage/PostLinkImages/{0}", data.ImageUrl);
        }
    }
}