using Svg;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Utils
{
    internal static class SvgToPngConverter
    {
        private const int DefaultIconSize = 50;

        public static BitmapImage GetBitmapImageBySvg(byte[] svgBytes, int width = DefaultIconSize, int height = DefaultIconSize)
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
                // Битый SVG от сервера - остаемся без иконки
                return null;
            }
        }

        private static BitmapImage ConvertBitmapToBitmapImage(Bitmap bitmap)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Сохранение Bitmap в поток
                bitmap.Save(memoryStream, ImageFormat.Png);
                memoryStream.Position = 0;

                // Создание BitmapImage из потока
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
