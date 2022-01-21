using System.Collections.Generic;
using System.Linq;
using Hulk.Shared;
using Namviet.Data;
using Namviet.Entites;
using Newtonsoft.Json;

namespace Namviet.Api
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DoanhThu" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DoanhThu.svc or DoanhThu.svc.cs at the Solution Explorer and start debugging.
    public class DoanhThu : IDoanhThu
    {
        public void DoWork(string data)
        {
            using (var db = new NamvietEntities()) InsertData(data, db);
        }

        private void InsertData(string data, NamvietEntities db)
        {
            var list = JsonConvert.DeserializeObject<IList<DoanhThuRecord>>(data);
            foreach (var record in list.Where(record => record.IsValid))
            {
                db.DoanhThus.Add(record.CopyTo(new Data.DoanhThu()));
                db.SaveChanges();
            }
        }
    }
}