using Ascon.Pilot.SDK;

namespace PilotLookUp.Model
{
    internal class LookUpModel
    {
        private IDataObject _dataObject { get; }

        public LookUpModel(IDataObject dataObject)
        {
            _dataObject = dataObject;
        }
    }
}