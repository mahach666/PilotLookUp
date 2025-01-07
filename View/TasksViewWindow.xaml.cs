using PilotLookUp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.View
{
    /// <summary>
    /// Логика взаимодействия для TasksViewWindow.xaml
    /// </summary>
    public partial class TasksViewWindow : Window
    {
        public TasksViewWindow(TreeNodeTask rootNode,IDataObject dataObject)
        {
            InitializeComponent();
            TreeViewObjects.ItemsSource = new List<TreeNodeTask> { rootNode };
            NameDisplayStart.Text = dataObject.DisplayName;
            IdStart.Text = dataObject.Id.ToString();

        }

        private void TreeViewObjects_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (e.NewValue is TreeNodeTask selectedNode)
            {
                // Отображаем свойства выбранного объекта
                PropertiesGrid.ItemsSource = selectedNode.DataObject
                    .GetType()
                    .GetProperties()
                    .Select(p => new { Property = p.Name, Value = p.GetValue(selectedNode.DataObject) });
            }
        }
    }
}
