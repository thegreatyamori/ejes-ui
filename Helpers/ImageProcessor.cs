using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace EjesUI.Helpers
{
    internal class ImageProcessor
    {
        public ImageProcessor() { }

        public static BitmapImage SetDefaultImage(string url = "https://via.placeholder.com/345x245.png")
        {
            return new BitmapImage(new Uri(url));
        }

        public static BitmapImage ProcessImage(string value)
        {
            string base64 = value.Split(",")[1];
            byte[] binaryData = Convert.FromBase64String(base64);

            BitmapImage bi = new();
            bi.BeginInit();
            bi.StreamSource = new MemoryStream(binaryData);
            bi.EndInit();

            return bi;
        }
    }
}
