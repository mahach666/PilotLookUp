using System;
using System.Collections.Concurrent;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects
{
    // Единая точка загрузки иконок типов из ресурсов сборки
    public static class TypeIcons
    {
        private static readonly ConcurrentDictionary<string, BitmapImage> Cache =
            new ConcurrentDictionary<string, BitmapImage>();

        public static BitmapImage Get(string iconName)
        {
            return Cache.GetOrAdd(iconName, name =>
            {
                var assemblyName = typeof(TypeIcons).Assembly.GetName().Name;
                var image = new BitmapImage(
                    new Uri($"pack://application:,,,/{assemblyName};component/Resources/TypeIcons/{name}.png"));
                image.Freeze();
                return image;
            });
        }
    }
}
