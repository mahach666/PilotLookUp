using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Model;
using PilotLookUp.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.VM
{
    internal class LookUpVM : INotifyPropertyChanged
    {
        internal LookUpView _view;
        private LookUpModel lookUpModel;


        public LookUpVM(LookUpModel lookUpModel)
        {
            this.lookUpModel = lookUpModel;
            DataObjectSelekted = SelectionDataObjects.FirstOrDefault();
        }


        public List<object> SelectionDataObjects => lookUpModel.SelectionDataObjects;
        private object _dataObjectSelekted { get; set; }
        public object DataObjectSelekted
        {
            get { return _dataObjectSelekted; }
            set
            {
                _dataObjectSelekted = value;
                OnPropertyChanged("Info");
                OnPropertyChanged();
            }
        }





        public ObjReflection Info => lookUpModel.GetInfo(_dataObjectSelekted);
        public ICommand SelectedValueClickCommand =>  new AsyncRelayCommand(lookUpModel.DataGridSelecror);



        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}