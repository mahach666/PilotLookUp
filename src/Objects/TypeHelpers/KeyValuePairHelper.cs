using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;
using PilotLookUp.Interfaces;
using Ascon.Pilot.SDK;

namespace PilotLookUp.Objects.TypeHelpers
{
    public class KeyValuePairHelper : PilotObjectHelper, IPilotObjectHelper
    {
        // Attr
        public KeyValuePairHelper(IThemeService themeService, KeyValuePair<string, object> keyValuePair, IDataObject sender)
            : base(themeService)
        {
            System.Diagnostics.Debug.WriteLine($"[TRACE] KeyValuePairHelper: Конструктор Attr вызван для ключа '{keyValuePair.Key}', значение типа {keyValuePair.Value?.GetType().FullName}");
            _lookUpObject = keyValuePair;
            _name = sender.Type.Attributes.FirstOrDefault(i => i.Name == keyValuePair.Key)?.Title ?? keyValuePair.Key;
            _isLookable = true;
            System.Diagnostics.Debug.WriteLine($"[TRACE] KeyValuePairHelper: _name = {_name}, _isLookable = {_isLookable}");
        }

        // TypesByChildren
        public KeyValuePairHelper(IThemeService themeService, KeyValuePair<Guid, int> keyValuePair)
            : base(themeService)
        {
            _lookUpObject = keyValuePair;
            _name = keyValuePair.Key.ToString();
            _isLookable = true;
        }
        // TypesByChildren v2
        public KeyValuePairHelper(IThemeService themeService, KeyValuePair<IDataObject, int> keyValuePair)
            : base(themeService)
        {
            _lookUpObject = keyValuePair;
            _name = keyValuePair.Key.DisplayName;
            _isLookable = true;
        }

        // Access
        public KeyValuePairHelper(IThemeService themeService, KeyValuePair<int, IAccess> keyValuePair)
            : base(themeService)
        {
            _lookUpObject = keyValuePair.Value;
            _name = keyValuePair.Value.AccessLevel.ToString();
            _isLookable = true;
        }

        // ITransition
        public KeyValuePairHelper(IThemeService themeService, KeyValuePair<Guid, IEnumerable<ITransition>> keyValuePair, IObjectsRepository objectsRepository)
            : base(themeService)
        {
            _lookUpObject = keyValuePair.Value;
            _name = objectsRepository?.GetUserStates().FirstOrDefault(i => i.Id == keyValuePair.Key)?.Name ?? PilotLookUp.Resources.Strings.InvalidName;
            _isLookable = true;
        }

        public override BitmapImage GetImage()
        {
            var kv = _lookUpObject;
            if (kv is KeyValuePair<string, object> strObj)
            {
                if (strObj.Value is string)
                    return new BitmapImage(new Uri("..\\..\\Resources\\TypeIcons\\stringIcon.png", UriKind.RelativeOrAbsolute));
                if (strObj.Value is int)
                    return new BitmapImage(new Uri("..\\..\\Resources\\TypeIcons\\intIcon.png", UriKind.RelativeOrAbsolute));
                if (strObj.Value is Guid)
                    return new BitmapImage(new Uri("..\\..\\Resources\\TypeIcons\\guidIcon.png", UriKind.RelativeOrAbsolute));
                if (strObj.Value is Enum)
                    return new BitmapImage(new Uri("..\\..\\Resources\\TypeIcons\\enumIcon.png", UriKind.RelativeOrAbsolute));
                if (strObj.Value is Ascon.Pilot.SDK.IDataObject)
                    return new BitmapImage(new Uri("..\\..\\Resources\\UI\\databaseEnabled.png", UriKind.RelativeOrAbsolute));
                if (strObj.Value is Ascon.Pilot.SDK.IAccess)
                    return new BitmapImage(new Uri("..\\..\\Resources\\TypeIcons\\accessIcon.png", UriKind.RelativeOrAbsolute));
                // fallback
                return new BitmapImage(new Uri("..\\..\\Resources\\TypeIcons\\attrIcon.png", UriKind.RelativeOrAbsolute));
            }
            // Для других кейсов — универсальная иконка
            return new BitmapImage(new Uri("..\\..\\Resources\\TypeIcons\\attrIcon.png", UriKind.RelativeOrAbsolute));
        }
    }
}
