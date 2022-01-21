using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Namviet.Api
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IDoanhThu" in both code and config file together.
    [ServiceContract(Name = "Namviet", Namespace = "http://namvietkhanhhoa.com/SOAP/DoanhThu", ConfigurationName = "Namviet")]
    public interface IDoanhThu
    {
        [OperationContract]
        void DoWork(string data);
    }
}
