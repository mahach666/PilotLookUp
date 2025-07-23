using Ascon.Pilot.Bim.SDK.Model;
using Ascon.Pilot.Bim.SDK.ModelStorage;
using Ascon.Pilot.Bim.SDK.ModelViewer;
using System;

namespace PilotLookUp.Model.Services
{
    public class BimModelService
    {
        private IModelManager _modelManager;
        private IModelStorageProvider _modelStorageProvider;
        private IModelViewer _modelViewer;

        public BimModelService(IModelManager modelManager,
         IModelStorageProvider modelStorageProvider)
        {
            _modelManager = modelManager;
            _modelStorageProvider = modelStorageProvider;
            Subscribe();
        }

        public IModelViewer CurentViewer => _modelViewer;
        public IModelStorage CurentStorage => _modelStorageProvider?.
            GetStorage(_modelViewer != null
            ? _modelViewer.ModelId
            : default)
            ?? null;

        public DateTime CurentVersion => _modelViewer != null
            ? _modelViewer.ModelVersion
            : default;

        private void Subscribe()
        {
            _modelManager.ModelLoaded += OnModelLoaded;
            _modelManager.ModelClosed += OnModelClosed;
            _modelManager.ModelOpened += OnModelLoaded;
        }

        private void OnModelLoaded(object sender, ModelEventArgs e)
        {
            _modelViewer = e.Viewer;
        }
        private void OnModelClosed(object sender, ModelEventArgs e)
        {
            if (_modelViewer != null && _modelViewer.ModelId == e.Viewer.ModelId)
            {
                _modelViewer = null;
            }
        }
    }
}
