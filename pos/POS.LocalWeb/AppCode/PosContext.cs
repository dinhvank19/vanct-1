using System.Web;
using System.Web.SessionState;
using POS.LocalWeb.Dal;
using POS.Shared;
using System.Drawing;
using System.Drawing.Printing;
using System;

namespace POS.LocalWeb.AppCode
{
    public static class PosContext
    {
        private static HttpSessionState Session => HttpContext.Current.Session;

        public static HttpServerUtility Server => HttpContext.Current.Server;

        public static string RequestTableNo => HttpContext.Current.Request["no"];

        public static bool RequestChangeTable => HttpContext.Current.Request["changeTable"] == "true";

        public static bool IsRefund => HttpContext.Current.Request["refund"] == "true";

        public static ReportUser User => HttpContext.Current.User.Identity.Name.JsonTextTo<ReportUser>();

        public static string UploadFolder => Server.MapPath("~/UploadManage");

        public static string IconOrList
        {
            set { Session["IconOrList"] = value; }
            get { return Session["IconOrList"] as string; }
        }

        public static void Print(string stringToPrint, string printerName)
        {
            var font = new Font("Times New Roman", 13.0f);
            using (var pd = new PrintDocument())
            {
                var with = pd.DefaultPageSettings.PrintableArea.Width;
                var height = pd.DefaultPageSettings.PrintableArea.Height;
                pd.PrinterSettings.PrinterName = printerName;
                pd.PrintPage += (sender, e) =>
                {
                    //using (var img = Image.FromFile(filePath))
                    //    e.Graphics.DrawImage(img, new Point(10, 10));

                    int charactersOnPage;
                    int linesPerPage;

                    // Sets the value of charactersOnPage to the number of characters 
                    // of stringToPrint that will fit within the bounds of the page.
                    e.Graphics.MeasureString(stringToPrint, font,
                        e.MarginBounds.Size, StringFormat.GenericTypographic,
                        out charactersOnPage, out linesPerPage);

                    e.Graphics.DrawString(stringToPrint,
                        font,
                        new SolidBrush(Color.Black),
                        new RectangleF(0, 0, with, height));
                };

                try
                {
                    pd.Print();
                }
                catch (Exception)
                {

                }
            }
        }
    }
}