using System.Collections.Generic;
using System.Linq;
using NamvietSender.NamvietApi;
using Newtonsoft.Json;

namespace NamvietSender
{
    public class NamvietRequester
    {
        public void SendData(IList<DoanhSo> list)
        {
            using (var client = new NamvietClient())
            {
                var jsonData = JsonConvert.SerializeObject(list);
                client.DoWork(jsonData);
            }
        }
    }
}