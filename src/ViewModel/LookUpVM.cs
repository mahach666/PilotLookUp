using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using Ascon.Pilot.SDK;
using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Input;
using PilotLookUp.Utils;
using Application = System.Windows.Application;

namespace PilotLookUp.ViewModel
{
    public class LookUpVM : INotifyPropertyChanged, IPage
    {
        private IRepoService _repoService;
        private IViewFactory _viewFactory;
        private IFileService _fileService;

        public LookUpVM(IRepoService lookUpModel, 
            IViewFactory viewFactory,
            IFileService fileService)
        {
            _repoService = lookUpModel;
            _viewFactory = viewFactory;
            _fileService = fileService;
            DataObjectSelected = SelectionDataObjects?.FirstOrDefault();
        }

        private List<ListItemVM> _selectionDataObjects;
        public List<ListItemVM> SelectionDataObjects
        {
            get => _selectionDataObjects;
            set
            {
                if (value == null || !value.Any()) return;
                _selectionDataObjects = value;
                DataObjectSelected = value?.FirstOrDefault();
                OnPropertyChanged();
                _ = UpdateFilteredDataObjectsAsync();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(PromptVisibility));
                _ = UpdateFilteredDataObjectsAsync();
            }
        }

        public Visibility PromptVisibility => string.IsNullOrEmpty(_searchText) ? Visibility.Visible : Visibility.Hidden;

        private List<ListItemVM> _filteredDataObjects;
        public List<ListItemVM> FilteredDataObjects
        {
            get => _filteredDataObjects;
            private set
            {
                _filteredDataObjects = value;
                OnPropertyChanged();
            }
        }

        public async Task UpdateFilteredDataObjectsAsync()
        {
            try
            {
                if (SearchText?.Length >= 2)
                {
                    var filtered = await Task.Run(() =>
                        SelectionDataObjects
                            .Where(i => i.ObjName.ToUpper().Contains(SearchText.ToUpper())
                                     || i.StrId.ToUpper().Contains(SearchText.ToUpper()))
                            .ToList()
                    );

                    Application.Current.Dispatcher.Invoke(() => FilteredDataObjects = filtered);
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(() => FilteredDataObjects = SelectionDataObjects);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private ListItemVM _dataObjectSelected;
        public ListItemVM DataObjectSelected
        {
            get => _dataObjectSelected;
            set
            {
                if (_dataObjectSelected != value)
                {
                    _dataObjectSelected = value;
                    if (_dataObjectSelected != null && (_dataObjectsSelected == null || _dataObjectsSelected.Count == 0))
                        DataObjectsSelected = new ArrayList { _dataObjectSelected };
                    if (_dataObjectSelected != null)
                        UpdateInfo();
                    else
                        Info = null;
                    OnPropertyChanged();
                }
            }
        }

        private IList _dataObjectsSelected;
        public IList DataObjectsSelected
        {
            get => _dataObjectsSelected;
            set
            {
                _dataObjectsSelected = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(OpenFileButtonVisibility));
                OnPropertyChanged(nameof(DownloadFilesButtonVisibility));
                CommandManager.InvalidateRequerySuggested();
            }
        }

        public Visibility OpenFileButtonVisibility =>
            DataObjectsSelected?.Count == 1 && GetSelectedFiles().Count == 1
                ? Visibility.Visible
                : Visibility.Collapsed;

        public Visibility DownloadFilesButtonVisibility =>
            GetSelectedFiles().Count >= 1
                ? Visibility.Visible
                : Visibility.Collapsed;

        private ObjectSet _dataGridSelected;
        public ObjectSet DataGridSelected
        {
            get => _dataGridSelected;
            set
            {
                _dataGridSelected = value;
                OnPropertyChanged();
            }
        }

        private async void UpdateInfo()
        {
            var selected = _dataObjectSelected;
            if (selected == null)
            {
                Info = null;
                return;
            }

            try
            {
                var info = await Task.Run(() => _repoService.GetObjInfo(selected.PilotObjectHelper));

                // Выделение могло смениться, пока грузилась информация
                if (ReferenceEquals(selected, _dataObjectSelected))
                    Info = info;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private List<ObjectSet> _info;
        public List<ObjectSet> Info
        {
            get => _info;
            set
            {
                _info = value;
                OnPropertyChanged();
            }
        }

        private void CopyToClipboard(string sender)
        {
            var errorText = "Упс, ничего не выбрано.";
            if (_dataObjectSelected == null)
            {
                MessageBox.Show(errorText);
                return;
            }

            try
            {
                if (sender == "List")
                {
                    Clipboard.SetText(_dataObjectSelected.PilotObjectHelper?.Name);
                }
                else if (sender == "DataGridSelectName")
                {
                    Clipboard.SetText(_dataGridSelected?.SenderMemberName);
                }
                else if (sender == "DataGridSelectValue")
                {
                    Clipboard.SetText(_dataGridSelected?.Description);
                }
                else if (sender == "DataGridSelectLine")
                {
                    Clipboard.SetText(_dataGridSelected?.SenderMemberName + "\t" + _dataGridSelected?.Description);
                }
                else
                {
                    MessageBox.Show(errorText);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private ICommand _copyCommand;
        public ICommand CopyCommand => _copyCommand ??= new RelayCommand<string>(CopyToClipboard);

        private ICommand _selectedValueCommand;
        public ICommand SelectedValueCommand => _selectedValueCommand ??= new RelayCommand<object>(_ => _viewFactory.LookSelection(_dataGridSelected));

        private ICommand _openSelectedFileCommand;
        public ICommand OpenSelectedFileCommand =>
            _openSelectedFileCommand ??= new RelayCommand<object>(_ => OpenSelectedFileAsync(), _ => OpenFileButtonVisibility == Visibility.Visible);

        private ICommand _downloadSelectedFilesCommand;
        public ICommand DownloadSelectedFilesCommand =>
            _downloadSelectedFilesCommand ??= new RelayCommand<object>(_ => DownloadSelectedFilesAsync(), _ => DownloadFilesButtonVisibility == Visibility.Visible);

        private List<IFile> GetSelectedFiles()
        {
            return (DataObjectsSelected?.Cast<object>() ?? Enumerable.Empty<object>())
                .OfType<ListItemVM>()
                .Select(i => i?.PilotObjectHelper?.LookUpObject)
                .OfType<IFile>()
                .ToList();
        }

        private async void OpenSelectedFileAsync()
        {
            var file = GetSelectedFiles().FirstOrDefault();
            if (file == null) return;

            try
            {
                var localPath = await Task.Run(() => _fileService.SaveFileToTemp(file));
                Process.Start(new ProcessStartInfo(localPath) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void DownloadSelectedFilesAsync()
        {
            var files = GetSelectedFiles();
            if (files.Count == 0) return;

            string folderPath = null;

            try
            {
                var owner = Application.Current?.MainWindow;
                var ownerHandle = owner != null ? new WindowInteropHelper(owner).Handle : IntPtr.Zero;

                var pickResult = FolderPicker.PickFolder(ownerHandle, "Выберите папку для скачивания", out folderPath);
                if (pickResult == FolderPicker.PickResult.Cancelled)
                    return;

                if (pickResult != FolderPicker.PickResult.Ok)
                {
                    using (var dialog = new System.Windows.Forms.FolderBrowserDialog
                    {
                        Description = "Выберите папку для скачивания",
                        ShowNewFolderButton = true
                    })
                    {
                        var result = dialog.ShowDialog();
                        if (result != System.Windows.Forms.DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath))
                            return;

                        folderPath = dialog.SelectedPath;
                    }
                }
            }
            catch
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(folderPath))
                return;

            try
            {
                await Task.Run(() => _fileService.SaveFilesToFolder(files, folderPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName() =>
             PagesName.LookUpPage;
    }
}
