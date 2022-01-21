using System;
using System.Globalization;
using System.Web;
using System.Web.Http;
using Hulk.Shared;

namespace Vanct.WebApp
{
    public class WebApiApplication : HttpApplication
    {
        private readonly object _icShopCountLock = new object();

        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        protected void Application_Start(object sender, EventArgs e)
        {
            Application["Online"] = 0;
            Application["CountVisit"] = 0;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            lock (_icShopCountLock)
            {
                // online count
                var online = (int) Application["Online"];
                online += 1;

                // count visit
                var visitFile = Server.MapPath("~/UploadManage/count_visit.txt");
                var countVisit = string.IsNullOrEmpty(visitFile.ReadFile().Trim())
                    ? 0
                    : visitFile.ReadFile().Trim().ToInt32();
                countVisit += 1;

                Application["CountVisit"] = countVisit;
                Application["Online"] = online;

                visitFile.WriteFile(countVisit.ToString(CultureInfo.InvariantCulture));
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected void Application_Error(object sender, EventArgs e)
        {
        }

        protected void Session_End(object sender, EventArgs e)
        {
            lock (_icShopCountLock)
            {
                // online count
                var online = (int) Application["Online"];
                online -= 1;
                Application["Online"] = online;
            }
        }

        protected void Application_End(object sender, EventArgs e)
        {
        }
    }
}