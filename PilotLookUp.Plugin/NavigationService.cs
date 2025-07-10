using Ascon.Pilot.SDK;
using PilotLookUp.Contracts;
using PilotLookUp.Enums;
using PilotLookUp.Interfaces;
using PilotLookUp.Core.Objects;
using PilotLookUp.UI.View;
using SimpleInjector;

namespace PilotLookUp.Plugin
{
    internal class NavigationService : INavigationService
    {
        private readonly Container _container;

        public NavigationService(Container container)
        {
            _container = container;
        }

        public void LookSelection(ObjectSet selectedObjects)
        {
            if (selectedObjects == null || !selectedObjects.IsLookable)
                return;
            ShowWindow(new StartViewInfo
            {
                PageName = PagesName.LookUpPage,
                SelectedObject = selectedObjects
            });
        }

        public void LookDB()
        {
            ShowWindow(new StartViewInfo { PageName = PagesName.DBPage });
        }

        public void SearchPage()
        {
            ShowWindow(new StartViewInfo { PageName = PagesName.SearchPage });
        }

        private void ShowWindow(StartViewInfo startInfo)
        {
            PilotLookUp.Plugin.DependencyInjection.SetStartViewInfo(startInfo);
            var window = _container.GetInstance<MainView>();
            window.Show();
        }
    }
} 