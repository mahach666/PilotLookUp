using PilotLookUp.Core;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IPilotObjectHelper
    {
        string Name { get; }
        string StringId { get; }
        object LookUpObject { get; }
        bool IsLookable { get; }
        ObjReflection Reflection { get; }
        Brush DefaultTextColor { get; }
        BitmapImage GetImage();
    }
} 