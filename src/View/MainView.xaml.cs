using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PilotLookUp.View
{
    public partial class MainView
    {
        public MainView(MainVM vm)
        {
            InitializeComponent();
            DataContext = vm;
            Closed += (s, e) => vm.Dispose();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Loaded -= OnLoaded;
            try
            {
                if (TryInjectPinNearCloseButton())
                    SidebarPin.Visibility = Visibility.Collapsed;
            }
            catch
            {
                // не удалось встроиться в шаблон окна - остаётся пин в сайдбаре
            }
        }

        // У DialogWindow из Ascon.Pilot.Theme нет точки расширения заголовка,
        // поэтому пин вставляется рядом с крестиком через визуальное дерево шаблона
        private bool TryInjectPinNearCloseButton()
        {
            var templateButtons = FindVisualChildren<Button>(this)
                .Where(b => !IsInsideContent(b))
                .ToList();

            // в шаблоне заголовка ожидаем ровно одну кнопку - крестик
            if (templateButtons.Count != 1)
                return false;

            var closeButton = templateButtons[0];
            if (!(VisualTreeHelper.GetParent(closeButton) is Panel panel))
                return false;

            var pin = CreateTitleBarPin(closeButton);

            // крестик подменяется на панель [пин, крестик] с тем же позиционированием
            var host = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = closeButton.HorizontalAlignment,
                VerticalAlignment = closeButton.VerticalAlignment
            };
            Grid.SetColumn(host, Grid.GetColumn(closeButton));
            Grid.SetRow(host, Grid.GetRow(closeButton));
            Grid.SetColumnSpan(host, Grid.GetColumnSpan(closeButton));
            Grid.SetRowSpan(host, Grid.GetRowSpan(closeButton));

            var index = panel.Children.IndexOf(closeButton);
            if (index < 0)
                return false;

            panel.Children.RemoveAt(index);
            host.Children.Add(pin);
            host.Children.Add(closeButton);
            panel.Children.Insert(index, host);
            return true;
        }

        private ToggleButton CreateTitleBarPin(Button closeButton)
        {
            var pin = new ToggleButton
            {
                ToolTip = "Поверх всех окон",
                Style = TryFindResource("PinToggleButton") as Style,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0)
            };

            if (closeButton.ActualWidth > 0 && closeButton.ActualHeight > 0)
            {
                pin.Width = closeButton.ActualWidth;
                pin.Height = closeButton.ActualHeight;
            }

            var assemblyName = typeof(MainView).Assembly.GetName().Name;
            pin.Content = new BitmapImage(
                new Uri($"pack://application:,,,/{assemblyName};component/Resources/UI/pin.png"));

            BindingOperations.SetBinding(pin, ToggleButton.IsCheckedProperty,
                new Binding(nameof(MainVM.IsTopmost)) { Mode = BindingMode.TwoWay });

            return pin;
        }

        private bool IsInsideContent(DependencyObject element)
        {
            var content = Content as DependencyObject;
            while (element != null)
            {
                if (element == content)
                    return true;
                element = VisualTreeHelper.GetParent(element);
            }
            return false;
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is T typed)
                    yield return typed;
                foreach (var nested in FindVisualChildren<T>(child))
                    yield return nested;
            }
        }
    }
}
