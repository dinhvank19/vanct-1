namespace POS.LocalWeb.Dal
{
    public class ReportGroup
    {
        public string Id { get; set; }
        public string Name { set; get; }
        public bool IsPrint { set; get; }
        public string Printer { set; get; }
    }

    public class ReportExGroup
    {
        public string Name { set; get; }
        public bool IsPrint { set; get; }
        public string Printer { set; get; }
        public bool IsTemporaryPrint { set; get; }
    }
}