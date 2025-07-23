using Ascon.Pilot.SDK;
using System.Linq;

namespace PilotLookUp
{
    public partial class App
    {
        private void ItemClick(string name)
        {
            if (name == "LookDB")
            {
                _viewFactory.LookDB();
                return;
            }
            else if (name == "Search")
            {
                _viewFactory.SearchPage();
                return;
            }

            if (_selectedService.Selected == null || !_selectedService.Selected.Any()) return;

            if (name == "LookSelected")
            {
                _viewFactory.LookSelection(_selectedService.Selected);
                return;
            }
        }

        // Event
        public void OnMenuItemClick(string name, MainViewContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, ObjectsViewContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, StorageContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, TasksViewContext2 context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, DocumentFilesContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, LinkedObjectsContext context) =>
            ItemClick(name);

        public void OnMenuItemClick(string name, LinkedTasksContext2 context) =>
            ItemClick(name);
    }
}
