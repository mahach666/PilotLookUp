using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Core;
using PilotLookUp.Extensions;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using PilotLookUp.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.ViewModel
{
    internal class LookUpVM : INotifyPropertyChanged
    {
        internal LookUpView _view;
        private LookUpModel _lookUpModel;
        private IObjectsRepository _objectsRepository { get; }
        public LookUpVM(LookUpModel lookUpModel, IObjectsRepository objectsRepository)
        {
            _lookUpModel = lookUpModel;
            DataObjectSelected = SelectionDataObjects.FirstOrDefault();
            _objectsRepository = objectsRepository;
        }

        public List<PilotObjectHelper> SelectionDataObjects => _lookUpModel.SelectionDataObjects;

        private PilotObjectHelper _dataObjectSelected;
        public PilotObjectHelper DataObjectSelected
        {
            get => _dataObjectSelected;
            set
            {
                if (_dataObjectSelected != value)
                {
                    _dataObjectSelected = value;
                    UpdateInfo();
                    OnPropertyChanged();
                }
            }
        }

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

        private void UpdateInfo()
        {
            Task.Run(async () =>
            {
                Info = await _lookUpModel.Info(_dataObjectSelected);
            });
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
            if (_dataObjectSelected == null) Clipboard.SetText("Ошибка копирования, ничего не выбрано");

            if (sender == "List")
            {
                Clipboard.SetText(_dataObjectSelected.Name);
            }
            else if (sender == "DataGridSelectName")
            {
                Clipboard.SetText(_dataGridSelected?.SenderMemberName);
            }
            else if (sender == "DataGridSelectValue")
            {
                Clipboard.SetText(_dataGridSelected?.Discription);
            }
            else if (sender == "DataGridSelectLine")
            {
                Clipboard.SetText(_dataGridSelected?.SenderMemberName + "\t" + _dataGridSelected?.Discription);
            }
            else
            {
                Clipboard.SetText("Ошибка копирования, ничего не выбрано");
            }
        }


        public ICommand CopyCommand => new RelayCommand<string>(CopyToClipboard);
        public ICommand SelectedValueCommand => new AsyncRelayCommand(_ => _lookUpModel.DataGridSelector(_dataGridSelected));
        public ICommand OpenAllTasks => new RelayCommand<string>(OpenTasksView);

        private async void OpenTasksView(string ad)
        {
            IDataObject objSelected = DataObjectSelected.LookUpObject as IDataObject;

            var rootNode = await BuildTree(objSelected);
            if (rootNode != null)
            {
                var window = new TasksViewWindow(rootNode, objSelected);
                window.ShowDialog();
            }
        }

        private async Task<TreeNodeTask> BuildTree(IDataObject obj)
        {
            // Если объекта нет, выводим ошибку
            if (obj == null)
            {
                MessageBox.Show("Задание не вложено в процесс", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }

            // Ищем самого верхнего родителя
            while (obj.ParentId != null && obj.ParentId.ToString() != "00000000-0000-0000-0000-000000000000")
            {
                obj = await _objectsRepository.GetObjectWithTimeout(obj.ParentId);
            }

            // Создаем корневой узел
            var rootNode = new TreeNodeTask(obj.Type.Title, obj.DisplayName, obj);

            // Рекурсивно добавляем детей
            await BuildChildNodes(rootNode);

            return rootNode;
        }

        private async Task BuildChildNodes(TreeNodeTask parentNode)
        {
            var children = await _objectsRepository.GetChildrensWithTimeout(parentNode.DataObject);  // Метод получения детей по ID

            foreach (var child in children)
            {

                var childNode = new TreeNodeTask(child.Type.Title, child.DisplayName, child);
                parentNode.Children.Add(childNode);
                BuildChildNodes(childNode); // Рекурсия для вложенных детей

            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
