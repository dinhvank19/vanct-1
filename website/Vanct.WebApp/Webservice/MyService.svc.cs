using System.ServiceModel;
using Newtonsoft.Json;
using Vanct.Dal.BO;
using Vanct.WebApp.AppCode;
using Vanct.WebApp.Report;

namespace Vanct.WebApp.Webservice
{
    [ServiceBehavior]
    public class MyService : IMyService
    {
        public void SyncData(string key, string data)
        {
            var user = ReportHelper.Get(key);
            if (user == null) return;

            //update tables
            var newData = JsonConvert.DeserializeObject<ReportUser>(data);
            foreach (var table in newData.Tables)
            {
                user.Tables.MergeItem(table);
            }

            //update total received
            user.Working = newData.Working;
        }

        public bool CheckOnlineStatus(string key, bool removeIfOffline)
        {
            var f = ReportHelper.CheckOnlineStatus(key);
            if (!f && removeIfOffline) ReportHelper.Remove(key);
            return f;
        }

        public void SyncDailyData(string key, string data)
        {
            //var newData = JsonConvert.DeserializeObject<ReportUser>(data);
            //VanctContext.ReportDao.AddWork(newData);
        }
    }
}