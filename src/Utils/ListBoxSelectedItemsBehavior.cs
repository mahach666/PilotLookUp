using Microsoft.Xaml.Behaviors;
using System.Collections;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace PilotLookUp.Utils
{
    public class ListBoxSelectedItemsBehavior : Behavior<ListBox>
    {
        public IList SelectedItems
        {
            get { return (IList)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(
                nameof(SelectedItems),
                typeof(IList),
                typeof(ListBoxSelectedItemsBehavior),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += OnSelectionChanged;
            AssociatedObject.Loaded += OnLoaded;
            UpdateSelectedItems();
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= OnSelectionChanged;
            AssociatedObject.Loaded -= OnLoaded;
            base.OnDetaching();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AssociatedObject.Dispatcher.BeginInvoke((System.Action)(() => UpdateSelectedItems()), DispatcherPriority.Loaded);
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateSelectedItems();
        }

        private void UpdateSelectedItems()
        {
            SelectedItems = AssociatedObject?.SelectedItems?.Cast<object>().ToList();
        }
    }
}
