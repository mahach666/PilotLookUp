using Ascon.Pilot.SDK;
using PilotLookUp.Commands;
using PilotLookUp.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Windows;
using IDataObject = Ascon.Pilot.SDK.IDataObject;

namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private List<IDataObject> _dataObjects { get; }
        private IObjectsRepository _objectsRepository { get; }


        public LookUpModel(List<IDataObject> dataObjects, IObjectsRepository objectsRepository)
        {
            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;
        }

        public List<IDataObject> SelectionDataObjects => _dataObjects;

        public ObjReflection GetInfo(IDataObject dataObject)
        {
            return new ObjReflection(dataObject);
        }

        public async Task DataGridSelecror(object obj)
        {
            //var loader = new ObjectLoader(_objectsRepository);

            if (obj == null) return;
            else if (obj is Guid id)
            {
                var loader = new ObjectLoader(_objectsRepository);
                IDataObject dataObj = await loader.Load(id);
                if (dataObj != null)
                {
                    new RiseCommand(new LookSeleсtion(new List<IDataObject>() { dataObj }, _objectsRepository));
                }
            }
            else if ( obj is IEnumerable<Guid> idEnum)
            {
                var dataObjes = new List<IDataObject>();
                foreach (var guid in idEnum)
                {
                    var loader = new ObjectLoader(_objectsRepository);
                    IDataObject dataObj = await loader.Load(guid);
                    if (dataObj != null)
                    {
                        dataObjes.Add(dataObj);
                    }
                }
                new RiseCommand(new LookSeleсtion(dataObjes, _objectsRepository));
            }
            else
            {
                MessageBox.Show(obj.GetType().ToString());
            }
        }
    }
}