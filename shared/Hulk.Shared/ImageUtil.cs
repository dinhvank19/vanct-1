using System.Drawing;
using System.Drawing.Drawing2D;

namespace IComm.Common
{
    public static class ImageUtil
    {
        #region Image helper

        public static Image ResizeImage(this Image imgToResize, Size size)
        {
            var sourceWidth = imgToResize.Width;
            var sourceHeight = imgToResize.Height;

            var nPercentW = (size.Width/(float) sourceWidth);
            var nPercentH = (size.Height/(float) sourceHeight);

            var nPercent = nPercentH < nPercentW ? nPercentH : nPercentW;

            var destWidth = (int) (sourceWidth*nPercent);
            var destHeight = (int) (sourceHeight*nPercent);

            var b = new Bitmap(destWidth, destHeight);
            using (var g = Graphics.FromImage(b))
            {
                g.InterpolationMode = InterpolationMode.High;

                g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
                return b;
            }

        }

        #endregion
    }
}
