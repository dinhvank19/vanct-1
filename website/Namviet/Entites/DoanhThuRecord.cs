using System;
using Namviet.Helpers;

namespace Namviet.Entites
{
    public class DoanhThuRecord
    {
        public int Id { get; set; }
        public int ExternalId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public decimal Price { get; set; }
        public double Amount { get; set; }
        public decimal Total => Price*(decimal) Amount;
        public bool IsValid => CreatedAt != null
                   && !string.IsNullOrEmpty(Name)
                   && !string.IsNullOrEmpty(Code);

        public string TotalAsText => ViewHelper.DisplayMoney(Total);
        public string PriceAsText => ViewHelper.DisplayMoney(Price);
        public string CreatedDateAsText => ViewHelper.DisplayDateTime(CreatedAt);
    }
}