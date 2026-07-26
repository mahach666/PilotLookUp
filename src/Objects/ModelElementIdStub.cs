using Ascon.Pilot.Bim.SDK;
using System;

namespace PilotLookUp.Objects
{
    // Собственная реализация IModelElementId: нужна, когда известны только Guid'ы
    // (например, свойства ElementId/ParentId внутренних типов вьюера),
    // а SDK-объекта идентификатора нет
    public sealed class ModelElementIdStub : IModelElementId
    {
        public ModelElementIdStub(Guid elementId, Guid modelPartId)
        {
            ElementId = elementId;
            ModelPartId = modelPartId;
        }

        public Guid ElementId { get; }
        public Guid ModelPartId { get; }
    }
}
