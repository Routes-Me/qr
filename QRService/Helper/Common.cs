using System;
using System.Drawing;
using System.IO;

namespace QRService.Helper
{
    public class Common
    {
        public static Byte[] BitmapToBytesCode(Bitmap image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
    }
}
