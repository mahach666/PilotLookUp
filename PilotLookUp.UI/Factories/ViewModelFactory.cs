using System;
using PilotLookUp.Core.Objects;
using PilotLookUp.Core.Interfaces;
using PilotLookUp.UI.ViewModel;
using SimpleInjector;

namespace PilotLookUp.UI.Factories
{
    public class ViewModelFactory : IViewModelFactory
    {
        private readonly Container _container;

        public ViewModelFactory(Container container)
        {
            _container = container;
        }

        public object CreateLookUpVM()
        {
            return _container.GetInstance<LookUpVM>();
        }

        public object CreateSearchVM()
        {
            return _container.GetInstance<SearchVM>();
        }

        public object CreateTaskTreeVM(PilotObjectHelper pilotObjectHelper)
        {
            var factory = _container.GetInstance<Func<PilotObjectHelper, TaskTreeVM>>();
            return factory(pilotObjectHelper);
        }

        public object CreateAttrVM(PilotObjectHelper pilotObjectHelper)
        {
            var factory = _container.GetInstance<Func<PilotObjectHelper, AttrVM>>();
            return factory(pilotObjectHelper);
        }

        public object CreateListItemVM(PilotObjectHelper pilotObjectHelper)
        {
            var factory = _container.GetInstance<Func<PilotObjectHelper, ListItemVM>>();
            return factory(pilotObjectHelper);
        }

        public object CreateSearchResVM(PilotObjectHelper pilotObjectHelper)
        {
            var factory = _container.GetInstance<Func<PilotObjectHelper, SearchResVM>>();
            return factory(pilotObjectHelper);
        }
    }
} 