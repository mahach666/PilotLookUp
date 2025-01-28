using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using System.Windows.Media;

namespace PilotLookUp.Objects
{
    public abstract class PilotObjectHelper
    {
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
                return new SolidColorBrush(App.Theme == Ascon.Pilot.Themes.ThemeNames.Jedi ? Colors.Black : Colors.White);
            }
        }
    }
}
