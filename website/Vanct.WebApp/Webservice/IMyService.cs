using System.ServiceModel;

namespace Vanct.WebApp.Webservice
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IMyService" in both code and config file together.
    [ServiceContract(Name = "Vanct", Namespace = "http://Vanct.vn/SOAP/Report", ConfigurationName = "Vanct")]
    public interface IMyService
    {
        [OperationContract]
        void SyncData(string key, string data);

        [OperationContract]
        bool CheckOnlineStatus(string key, bool removeIfOffline);

        [OperationContract]
        void SyncDailyData(string key, string data);
    }
}