using System.Linq;

namespace NamvietSender
{
    class Program
    {
        private static void Main(string[] args)
        {
            var reader = new AccessReader();
            var list = reader.GetDoanhThu();
            new NamvietRequester().SendData(list);
            if (list.Any())
            {
                var endPostion = list.Max(i => i.ExternalId);
                reader.SetPosition(endPostion);
            }
        }
    }
}
