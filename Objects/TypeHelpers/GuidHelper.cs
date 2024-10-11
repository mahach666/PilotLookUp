using Ascon.Pilot.SDK;
using PilotLookUp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace PilotLookUp.Objects.TypeHelpers
{
    internal class GuidHelper : PilotObjectHelper
    {
        public GuidHelper(Guid obj, IObjectsRepository objectsRepository)
        {
            _objectsRepository = objectsRepository;

            _obj = obj;
            var thread = new Thread(ObjectGetter);
            thread.Start();

            Thread.Sleep(5000);

            Name = LookUpObject.ToString();
        }

        private async void ObjectGetter()
        {
            LookUpObject = await _objectsRepository.GetObject(_obj);
            var a = 5;
        }

        private IObjectsRepository _objectsRepository { get; }
        private Guid _obj { get; }
    }
}
