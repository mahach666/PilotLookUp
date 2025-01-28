using Svg;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

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

            if (File.Exists(path))
            {
                byte[] oldPngBytes = File.ReadAllBytes(path);
                if (oldPngBytes.SequenceEqual(newPngBytes))
                {
                    return path;
                }
                File.Delete(path);
            }

            File.WriteAllBytes(path, newPngBytes); // Сохранение PNG
            return path;
        }

        //private static byte[] ConvertSvgToPng(byte[] svgBytes, int width = 50, int height = 50)
        //{
        //    // Преобразуем байты SVG в строку
        //    var svgString = System.Text.Encoding.UTF8.GetString(svgBytes);

        //    // Загружаем SVG
        //    using var svg = new SKSvg();
        //    svg.Load(svgString);

        //    // Устанавливаем размеры
        //    var svgPicture = svg.Picture;
        //    var info = new SKImageInfo(width, height);

        //    // Рисуем PNG
        //    using var surface = SKSurface.Create(info);
        //    var canvas = surface.Canvas;
        //    canvas.Clear(SKColors.Transparent); // Прозрачный фон
        //    canvas.DrawPicture(svgPicture);
        //    canvas.Flush();

        //    // Получаем PNG в байтах
        //    using var image = surface.Snapshot();
        //    using var data = image.Encode(SKEncodedImageFormat.Png, 100);
        //    return data.ToArray();
        //}

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
