using Ascon.Pilot.SDK;
using Ascon.Pilot.Themes;
using PilotLookUp.Domain.Entities;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IViewDirectorService
    {
        void LookSelection(ObjectSet selectedObjects, IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider, ThemeNames theme);
        void LookDB(IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider, ThemeNames theme);
        void SearchPage(IObjectsRepository objectsRepository, ITabServiceProvider tabServiceProvider, ThemeNames theme);
    }
} 