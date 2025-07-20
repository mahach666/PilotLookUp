using PilotLookUp.Contracts;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.View;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Model.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly IPageService _pageServiceFactory;

        public ViewFactory(IPageService pageServiceFactory)
        {
            _pageServiceFactory = pageServiceFactory;
        }

        public void LookSelection(ObjectSet selected)
        {
            if (!selected.IsLookable) return;
            Show(new StartViewInfo { PageName = PagesName.LookUpPage, SelectedObject = selected });
        }

        public void LookDB() => Show(new StartViewInfo { PageName = PagesName.DBPage });
        public void SearchPage() => Show(new StartViewInfo { PageName = PagesName.SearchPage });

        private void Show(StartViewInfo info)
        {
            //_pageServiceFactory.Initialize(info);

            //var vm = new MainVM(_pageServiceFactory);
            //var window = new MainView(vm);

            //window.Show();
        }
    }
}
