using Svg;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace PilotLookUp.Utils
{
    internal static class SvgToPngConverter
    {
        public static string SaveSvgToPng(byte[] svgBytes, string fileName, int width = 50, int height = 50)
        {
            if (svgBytes == null || svgBytes.Length == 0) return "";
            if (!fileName.Contains(".png")) fileName += ".png";

            string path = Path.Combine(Path.GetTempPath(), fileName);

            byte[] newPngBytes = ConvertSvgToPng(svgBytes, width, height); // Преобразование
            File.WriteAllBytes(path, newPngBytes); // Сохранение PNG
            return path;
        }

        private static byte[] ConvertSvgToPng(byte[] svgBytes, int width = 50, int height = 50)
        {
            // Преобразуем байты SVG в строку
            var svgString = System.Text.Encoding.UTF8.GetString(svgBytes);

            // Загружаем SVG-документ
            var svgDocument = SvgDocument.FromSvg<SvgDocument>(svgString);

            // Устанавливаем размеры
            svgDocument.Width = width;
            svgDocument.Height = height;

            // Создаем растровое изображение
            using var bitmap = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.Transparent); // Прозрачный фон
                svgDocument.Draw(graphics);       // Рисуем SVG на Graphics
            }

            // Конвертируем растровое изображение в PNG
            using var memoryStream = new MemoryStream();
            bitmap.Save(memoryStream, ImageFormat.Png);
            return memoryStream.ToArray();
        }
    }
}
