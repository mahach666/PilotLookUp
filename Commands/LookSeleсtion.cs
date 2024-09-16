using Ascon.Pilot.SDK;
using PilotLookUp.Model;
using PilotLookUp.View;
using PilotLookUp.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IDataObject = Ascon.Pilot.SDK.IDataObject;


namespace PilotLookUp.Commands
{
    internal class LookSeleсtion : ICommand
    {
        private List<IDataObject> _dataObject { get; }
        private IObjectsRepository _objectsRepository { get; }

        internal  LookSeleсtion(List<IDataObject> dataObject, IObjectsRepository objectsRepository)
        {
            _dataObject = dataObject;
            _objectsRepository = objectsRepository;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            LookUpView view = new LookUpView(new LookUpVM(new LookUpModel(_dataObject, _objectsRepository)));
            view.Show();
        }
    }
}
