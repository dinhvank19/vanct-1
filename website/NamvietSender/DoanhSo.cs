using System;

namespace NamvietSender
{
    public class DoanhSo
    {
        public int ExternalId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Price { get; set; }
        public double Amount { get; set; }
    }
}
