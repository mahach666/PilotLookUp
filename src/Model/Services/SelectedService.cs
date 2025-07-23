using Ascon.Pilot.SDK;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class SelectedService : ISelectedService
    {
        private IObjectsRepository _objectsRepository;
        private ObjectSet _convertSelection;

        public SelectedService(IObjectsRepository objectsRepository)
        {
            _objectsRepository = objectsRepository;
        }

        public ObjectSet Selected => _convertSelection;

        public void UpdateSelected(MarshalByRefObject context)
        {
            var map = new PilotObjectMap(_objectsRepository);

            IEnumerable<object>? raw = context switch
            {
                ObjectsViewContext c => c.SelectedObjects?.Cast<object>(),
                DocumentFilesContext c => c.SelectedObjects?.Cast<object>(),
                LinkedObjectsContext c => c.SelectedObjects?.Cast<object>(),
                StorageContext c => c.SelectedObjects?.Cast<object>(),
                TasksViewContext2 c => c.SelectedTasks?.Cast<object>(),
                LinkedTasksContext2 c => c.SelectedTasks?.Cast<object>(),
                DocumentAnnotationsListContext с => с.SelectedAnnotations?.Cast<object>(),

                _ => null
            };

            if (raw?.Any() == true)
            {
                _convertSelection = new ObjectSet(null);
                _convertSelection.AddRange(raw.Select(map.Wrap));
            }
        }
    }
}
