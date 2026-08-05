using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace PilotLookUp.View.CustomUIElements
{
    // Выпадающий список с полем поиска: замена ComboBox для длинных справочников
    // (атрибуты, типы, люди). Выбор одиночный, фильтр по подстроке без учета регистра
    public partial class SearchableDropDown : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            nameof(ItemsSource), typeof(IEnumerable), typeof(SearchableDropDown),
            new PropertyMetadata(null, OnItemsSourceChanged));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            nameof(SelectedItem), typeof(object), typeof(SearchableDropDown),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedItemChanged));

        public static readonly DependencyProperty PlaceholderProperty = DependencyProperty.Register(
            nameof(Placeholder), typeof(string), typeof(SearchableDropDown),
            new PropertyMetadata("выберите...", OnSelectedItemChanged));

        private ListCollectionView _view;

        public SearchableDropDown()
        {
            InitializeComponent();
            UpdateDisplayText();
        }

        public IEnumerable ItemsSource
        {
            get => (IEnumerable)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set => SetValue(SelectedItemProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (SearchableDropDown)d;
            var items = (e.NewValue as IEnumerable)?.Cast<object>().ToList() ?? new List<object>();

            control._view = new ListCollectionView(items) { Filter = control.FilterItem };
            control.List.ItemsSource = control._view;
        }

        private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((SearchableDropDown)d).UpdateDisplayText();
        }

        private bool FilterItem(object item)
        {
            var filter = FilterBox?.Text;
            if (string.IsNullOrWhiteSpace(filter))
                return true;

            return item?.ToString()?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void UpdateDisplayText()
        {
            DisplayTextBlock.Text = SelectedItem?.ToString() ?? Placeholder;
        }

        // Попап управляется кодом: TwoWay-биндинг IsOpen на IsChecked через
        // ElementName внутри DataTemplate работает ненадежно
        private void OnToggleClick(object sender, RoutedEventArgs e)
        {
            DropPopup.IsOpen = Toggle.IsChecked == true;
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            Toggle.IsChecked = false;
        }

        private void OnPopupOpened(object sender, EventArgs e)
        {
            FilterBox.Text = string.Empty;
            _view?.Refresh();
            FilterBox.Focus();
            Keyboard.Focus(FilterBox);
        }

        private void OnFilterTextChanged(object sender, TextChangedEventArgs e)
        {
            _view?.Refresh();
        }

        private void OnListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Реагируем только на выбор пользователем; сброс выделения при
            // перефильтрации приходит с пустым AddedItems и игнорируется
            if (e.AddedItems.Count == 0)
                return;

            SelectedItem = e.AddedItems[0];
            DropPopup.IsOpen = false;
            List.SelectedItem = null;
        }
    }
}
