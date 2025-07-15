using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;
using PilotLookUp.ViewModel;
using PilotLookUp.Domain.Helpers;

namespace PilotLookUp.Model
{
    public class EqualityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType,
                              object parameter, CultureInfo culture)
            => values.Length == 2 && Equals(values[0], values[1]);

        public object[] ConvertBack(object value, Type[] targetTypes,
                                    object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

    public class ButtonVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // parameter: "Task" или "Attr"
            var selectedControl = value;
            if (selectedControl is LookUpVM lookUpVM)
            {
                if (lookUpVM.DataObjectSelected?.PilotObjectHelper?.LookUpObject is IDataObject ||
                    lookUpVM.DataObjectSelected?.PilotObjectHelper is DataObjectHelper)
                {
                    return Visibility.Visible;
                }
            }
            else if (selectedControl is TaskTreeVM || selectedControl is AttrVM)
            {
                return Visibility.Visible;
            }
            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }

}
