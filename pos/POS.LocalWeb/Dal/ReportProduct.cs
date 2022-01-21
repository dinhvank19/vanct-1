using POS.Shared;

namespace POS.LocalWeb.Dal
{
    public class ReportProduct
    {
        public string Id { get; set; }
        public string Name { set; get; }
        public string SearchName { set; get; }
        public string Group { set; get; }
        public string Muc { set; get; }
        public double Price { set; get; }
        public string PriceText => Price.ToMoney();
        public string Dvt { get; set; }
        public string ImagePath { get; set; }

        public string BackgroundPhoto
        {
            get
            {
                return string.IsNullOrEmpty(ImagePath)
                    ? null
                    : string.Format("style=\"background-image: url(/UploadManage/{0})\"", ImagePath);
            }
        }
    }
}