using Ascon.Pilot.Bim.SDK;
using System;
using System.Collections.Generic;

namespace PilotLookUp.Objects
{
    // Снимок элемента модели с его свойствами: хранилище недоступно из хелперов,
    // поэтому элемент и свойства загружаются в момент выделения
    public sealed class BimElementView
    {
        public BimElementView(
            IModelElementId id,
            IModelElement element,
            IReadOnlyList<IPropertySet> propertySets,
            string loadError)
        {
            Id = id;
            Element = element;
            PropertySets = propertySets ?? Array.Empty<IPropertySet>();
            LoadError = loadError;
        }

        public IModelElementId Id { get; }
        public IModelElement Element { get; }
        public IReadOnlyList<IPropertySet> PropertySets { get; }
        public string LoadError { get; }
    }
}
