using System;

namespace Vanct.Dal.BO
{
    public class PosDevice
    {
        public int Id { get; set; }
        public string UniqueId { get; set; }
        public int CompanyId { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string DeviceUuid { get; set; }
        public string DeviceName { get; set; }
        public DateTime? FirstLoginDate { get; set; }
        public DateTime? LastLoginDate { get; set; }
    }
}