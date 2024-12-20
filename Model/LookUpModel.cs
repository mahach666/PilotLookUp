using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Core;
using PilotLookUp.Extensions;
using PilotLookUp.Model.Utils;
using PilotLookUp.Objects;
using PilotLookUp.ViewBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Threading.Tasks;
using System.Windows;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private List<PilotObjectHelper> _dataObjects { get; }
        private IObjectsRepository _objectsRepository { get; }

        //private ObjectLoader _loader { get; }
        //private PilotObjectHelper _dataObjectSelected { get; set; }

        public LookUpModel(List<PilotObjectHelper> dataObjects, IObjectsRepository objectsRepository)
        {
            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;
            //_loader = new ObjectLoader(_objectsRepository);
        }

        public List<PilotObjectHelper> SelectionDataObjects => _dataObjects;


        public async Task DataGridSelector(PilotObjectHelper sender ,object obj)
        {
            if (obj == null) return;
            new Tracer().Trace(_objectsRepository, sender, obj);
        }
    }
}