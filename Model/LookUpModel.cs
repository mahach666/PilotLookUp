using Ascon.Pilot.SDK;
using PilotLookUp.Model.Utils;
using PilotLookUp.Objects;
using PilotLookUp.ViewBuilders;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private ObjectSet _dataObjects { get; }
        private IObjectsRepository _objectsRepository { get; }

        public LookUpModel(ObjectSet dataObjects, IObjectsRepository objectsRepository)
        {
            _dataObjects = dataObjects;
            _objectsRepository = objectsRepository;
        }

        public ObjectSet SelectionDataObjects => _dataObjects;

        public async Task<List<TreeViewItem>> GetExampleTree()
        {
            var res = new List<TreeViewItem>();

            foreach (var obj in _dataObjects)
            {
                var main = new TreeViewItem();

                main.Header = obj.Name;
                main.Tag = obj;

                res.Add(main);

                ObjectSet newPilotObj = await new Tracer(_objectsRepository, obj, null).Trace(((IDataObject)obj.LookUpObject).Children);

                foreach (var child in newPilotObj)
                {
                    var childItem = new TreeViewItem { Header = child.Name, Tag = child };
                    main.Items.Add(childItem);
                }
            }
            return res;
        }


        public async Task DataGridSelector(ObjectSet obj)
        {
            if (obj == null) return;
            new LookSeleсtion(obj, _objectsRepository);
        }

        public async Task<List<ObjectSet>> Info(PilotObjectHelper sender)
        {
            var res = new List<ObjectSet>();

            foreach (var pair in sender.Reflection.KeyValuePairs)
            {
                ObjectSet newPilotObj = await new Tracer(_objectsRepository, sender, pair.Key).Trace(pair.Value);
                res.Add(newPilotObj);
            }

            return res;
        }
    }
}