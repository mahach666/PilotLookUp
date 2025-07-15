using PilotLookUp.Domain.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PilotLookUp.ViewModel
{
    public abstract class BaseValidatedViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected BaseValidatedViewModel(IValidationService validationService, params object[] dependencies)
        {
            validationService?.ValidateConstructorParams(dependencies);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
} 