using Ascon.Pilot.SDK;
using PilotLookUp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects
{
    public abstract class PilotObjectHelper
    {
        public PilotObjectHelper(IObjectsRepository objectsRepository)
        {
            ObjectsRepository = objectsRepository;
        }
        // Отображаемое "Красивое" имя
        protected string _name { get; set; }
        public string Name { get => _name; }

        // Обьект Pilot
        protected object _lookUpObject { get; set; }
        public object LookUpObject { get => _lookUpObject; }

        // Можно ли заглянуть в объект
        protected bool _isLookable { get; set; }
        public bool IsLookable { get => _lookUpObject != null ? _isLookable : false; }
        // DB Pilot
        public IObjectsRepository ObjectsRepository { get; }

        // Рефлексия обьекта
        public ObjReflection Reflection { get { return LookUpObject == null ? ObjReflection.Empty() : new ObjReflection(this); } }
    }
}
