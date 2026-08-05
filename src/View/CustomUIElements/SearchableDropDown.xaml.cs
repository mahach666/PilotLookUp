using PilotLookUp.Model.AdvancedSearch;
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

            // Схема захвата мыши как у ComboBox: пока попап открыт, держим
            // SubTree-захват на контроле; клик вне поддерева закрывает попап
            AddHandler(Mouse.PreviewMouseDownOutsideCapturedElementEvent,
                new MouseButtonEventHandler(OnOutsideMouseDown), true);

            // Текстбокс фильтра и скроллбар списка перехватывают захват на время
            // своих жестов - после отпускания кнопки возвращаем его контролу
            PopupRoot.AddHandler(PreviewMouseUpEvent,
                new MouseButtonEventHandler(OnPopupPreviewMouseUp), true);
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

            // У элемента может быть текст поиска шире подписи (прочие заголовки атрибута)
            var text = (item as ISearchableText)?.SearchText ?? item?.ToString();
            return text?.IndexOf(filter, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void UpdateDisplayText()
        {
            DisplayTextBlock.Text = SelectedItem?.ToString() ?? Placeholder;
        }

        // Попап управляется кодом. Открытие строго на ClickMode=Press с захватом
        // SubTree: без захвата Popup(StaysOpen=False) закрывается уже на отпускании
        // кнопки мыши над тогглом - "выпадашка не кликабельна"
        private void OnToggleClick(object sender, RoutedEventArgs e)
        {
            if (Toggle.IsChecked == true)
            {
                DropPopup.IsOpen = true;
                Mouse.Capture(this, CaptureMode.SubTree);
            }
            else
            {
                DropPopup.IsOpen = false;
            }
        }

        private void OnOutsideMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (DropPopup.IsOpen)
                DropPopup.IsOpen = false;
        }

        private void OnPopupPreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Вернуть захват после жеста вложенного элемента (выделение текста,
            // перетаскивание скроллбара), иначе клик мимо перестанет закрывать попап
            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (DropPopup.IsOpen && Mouse.Captured == null)
                    Mouse.Capture(this, CaptureMode.SubTree);
            }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private void OnPopupClosed(object sender, EventArgs e)
        {
            Toggle.IsChecked = false;
            if (ReferenceEquals(Mouse.Captured, this))
                Mouse.Capture(null);
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
