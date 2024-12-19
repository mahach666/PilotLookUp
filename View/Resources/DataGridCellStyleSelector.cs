using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using PilotLookUp.Objects;

namespace PilotLookUp.View.Resources
{
    public sealed class DataGridCellStyleSelector : DataTemplateSelector
    {
        /// <summary>
        ///     Data grid cell style selector
        /// </summary>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is null) return null;

            var descriptor = (PilotObjectHelper)item;
            var presenter = (FrameworkElement)container;
            var templateName = descriptor.Name; //switch
            //{
            //    ColorDescriptor => "DataGridColorCellTemplate",
            //    ColorMediaDescriptor => "DataGridColorCellTemplate",
            //    _ => "DefaultLookupDataGridCellTemplate"
            //};

            return (DataTemplate)presenter.FindResource(templateName);
        }
    }
}
