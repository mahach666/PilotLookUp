using Ascon.Pilot.SDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Model
{
    public class TreeNodeTask
    {
        public string DisplayName { get; set; }
        public List<TreeNodeTask> Children { get; set; } = new List<TreeNodeTask>();
        public IDataObject DataObject { get; set; }

        public string PngPath { get; private set; } // Путь к PNG-иконке
        public TreeNodeTask(string typeName, string displayName, IDataObject dataObject)
        {
            DisplayName = $"Тип: {typeName}\nИмя: {displayName}";
            DataObject = dataObject;
            // Если есть SVG-данные, преобразуем их в PNG
            if (dataObject.Type.SvgIcon != null)
            {
                PngPath = ConvertSvgToPng(dataObject.Type.SvgIcon);
            }
        }

        private string ConvertSvgToPng(byte[] svgData)
        {
            // Путь для временного PNG-файла
            string tempPngPath = Path.GetTempFileName() + ".png";

            // Преобразуем SVG в PNG
            using (var stream = new MemoryStream(svgData))
            {
                // Загружаем SVG-документ
                var svgDocument = Svg.SvgDocument.Open<Svg.SvgDocument>(stream);

                // Рендерим PNG
                using (var bitmap = svgDocument.Draw())
                {
                    // Сохраняем PNG-файл
                    bitmap.Save(tempPngPath, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
            return tempPngPath;
        }
    }
}
