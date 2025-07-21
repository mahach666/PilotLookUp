using PilotLookUp.Contracts;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.View;

namespace PilotLookUp.Model.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly IViewModelFactory _viewModelFactory;

        public ViewFactory( IViewModelFactory viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void LookSelection(ObjectSet selected)
        {
            if (!selected.IsLookable) return;
            var startInfo = new StartViewInfo { PageName = PagesName.LookUpPage, SelectedObject = selected };
            Show(startInfo);
        }

        public void LookDB() 
        {
            var startInfo = new StartViewInfo { PageName = PagesName.DBPage };
            Show(startInfo);
        }

        public void SearchPage() 
        {
            var startInfo = new StartViewInfo { PageName = PagesName.SearchPage };
            Show(startInfo);
        }

        private void Show(StartViewInfo info)
        {
            var viewModel = _viewModelFactory.CreateMainVM(info);
            var window = new MainView(viewModel);
            window.Show();
        }
    }
}
