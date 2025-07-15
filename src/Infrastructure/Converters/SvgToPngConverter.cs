using Svg;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Infrastructure.Converters
{
    internal static class SvgToPngConverter
    {
        public static string SaveSvgToPng(byte[] svgBytes, string fileName, int width = 50, int height = 50)
        {
            if (svgBytes == null || svgBytes.Length == 0) return "";
            if (!fileName.Contains(".png")) fileName += ".png";

            string path = Path.Combine(Path.GetTempPath(), fileName);

            byte[] newPngBytes = ConvertSvgToPng(svgBytes, width, height); 
            File.WriteAllBytes(path, newPngBytes); 
            return path;
        }

        public static BitmapImage GetBitmapImageBySvg(byte[] svgBytes, int width = 50, int height = 50)
        {
            if (svgBytes == null || svgBytes.Length == 0) return null;
            try
            {
                var svgString = System.Text.Encoding.UTF8.GetString(svgBytes);
                var svgDocument = SvgDocument.FromSvg<SvgDocument>(svgString);
                svgDocument.Width = width;
                svgDocument.Height = height;
                using var bitmap = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.Transparent);
                    svgDocument.Draw(graphics);
                }
                return ConvertBitmapToBitmapImage(bitmap);
            }
            catch
            {
                return null;
            }
        }

        private static byte[] ConvertSvgToPng(byte[] svgBytes, int width = 50, int height = 50)
        {
            var svgString = System.Text.Encoding.UTF8.GetString(svgBytes);

            var svgDocument = SvgDocument.FromSvg<SvgDocument>(svgString);

            svgDocument.Width = width;
            svgDocument.Height = height;

            using var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Transparent); 
                svgDocument.Draw(graphics);      
            }

            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }

        private static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                return bitmapImage;
            }
        }
    }
}
