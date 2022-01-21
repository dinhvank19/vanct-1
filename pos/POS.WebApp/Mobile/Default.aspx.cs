using System;
using System.Web.UI;
using POS.Dal.Enums;
using POS.WebApp.AppCode;

namespace POS.WebApp.Mobile
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            // get inprogress session
            var user = PosContext.User;

            // if existing session -> go to page OrderOverview
            if (user.Session != null)
                Response.Redirect("~/Mobile/OrderOverview.aspx");
        }

        protected void BtnMorning(object sender, EventArgs e)
        {
            var user = PosContext.User;
            user.Session = PosContext.BizSession.Create(WorkingTime.Morning, user.Id);
            Response.Redirect("~/Mobile/OrderOverview.aspx");
        }

        protected void BtnAfternoon(object sender, EventArgs e)
        {
            var user = PosContext.User;
            user.Session = PosContext.BizSession.Create(WorkingTime.Afternoon, user.Id);
            Response.Redirect("~/Mobile/OrderOverview.aspx");
        }

        protected void BtnEvening(object sender, EventArgs e)
        {
            var user = PosContext.User;
            user.Session = PosContext.BizSession.Create(WorkingTime.Evening, user.Id);
            Response.Redirect("~/Mobile/OrderOverview.aspx");
        }
        
    }
}