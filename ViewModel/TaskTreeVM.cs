using PilotLookUp.Commands;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Model;
using PilotLookUp.Objects;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace PilotLookUp.ViewModel
{
    internal class TaskTreeVM : INotifyPropertyChanged, IPage
    {
        private LookUpModel _lookUpModel;
        private PilotObjectHelper _objectHelper;
        public TaskTreeVM(LookUpModel lookUpModel, PilotObjectHelper pilotObjectHelper)
        {
            _lookUpModel = lookUpModel;
            _objectHelper = pilotObjectHelper;
            Task.Run(async () =>
            {
                LastParrent = await _lookUpModel.FindLastParrent(_objectHelper);
                ListItemVM rootNode = new ListItemVM(LastParrent);
                rootNode =await lookUpModel.FillChild(rootNode);
                FirstParrentNode = new ObservableCollection<ListItemVM> { rootNode };

            });
        }

        #region Информация о выбранном задании
        public string IdSelectedItem
        {
            get
            {
                return _objectHelper.StringId;
            }
        }
        public string NameSelectedItem
        {
            get
            {
                return _objectHelper.Name;
            }
        }
        #endregion

        #region Дерево процесса

        private PilotObjectHelper LastParrent { get;set; }

        private ObservableCollection<ListItemVM> _firstParrentNode;

        public ObservableCollection<ListItemVM> FirstParrentNode
        {

            get => _firstParrentNode;
            set
            {
                _firstParrentNode = value;
                OnPropertyChanged();
            }
        }
        #endregion




        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        PagesName IPage.GetName()
        {
            return PagesName.TaskTree;
        }
    }
}
