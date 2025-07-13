using PilotLookUp.Core;
using PilotLookUp.Interfaces;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PilotLookUp.Objects
{
    public abstract class PilotObjectHelper
    {
        private static IThemeService _themeService;

        // Отображаемое "Красивое" имя
        protected string _name { get; set; }
        public string Name { get => _name; }

        // Id или Guid в строковом виде
        protected string _stringId { get; set; }
        public string StringId { get => _stringId; }

        // Обьект Pilot
        protected object _lookUpObject { get; set; }
        public object LookUpObject { get => _lookUpObject; }

        // Можно ли заглянуть в объект
        protected bool _isLookable { get; set; }
        public bool IsLookable { get => _lookUpObject != null ? _isLookable : false; }

        // Рефлексия обьекта
        public ObjReflection Reflection { get { return LookUpObject == null ? ObjReflection.Empty() : new ObjReflection(this); } }

        // Цвет текста в листБоксе
        public Brush DefaultTextColor
        {
            get
            {
                var themeService = _themeService ?? App.ThemeService;
                return new SolidColorBrush(themeService.IsJediTheme ? Colors.Black : Colors.White);
            }
        }

        // Метод для установки ThemeService (вызывается из DI контейнера)
        public static void SetThemeService(IThemeService themeService)
        {
            _themeService = themeService;
        }

        abstract public BitmapImage GetImage();
    }
}
