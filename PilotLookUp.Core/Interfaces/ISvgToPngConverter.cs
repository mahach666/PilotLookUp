using System.Windows.Media.Imaging;

namespace PilotLookUp.Core.Interfaces
{
    public interface ISvgToPngConverter
    {
        BitmapImage GetBitmapImageBySvg(string svgIcon);
    }
} 