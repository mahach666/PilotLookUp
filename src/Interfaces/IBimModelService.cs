using Ascon.Pilot.Bim.SDK;
using Ascon.Pilot.Bim.SDK.ModelStorage;
using Ascon.Pilot.Bim.SDK.ModelViewer;
using PilotLookUp.Objects;
using System;

namespace PilotLookUp.Interfaces
{
    public interface IBimModelService
    {
        // false в обычном Pilot-ICE (BIM-сервисы клиентом не предоставлены)
        bool IsAvailable { get; }

        // null, если BIM недоступен или хранилище не найдено
        IModelStorage GetStorage(Guid modelId);

        // null, если вьюер с такой вкладкой не отслеживается
        IModelViewer TryGetViewer(Guid tabId);

        // Элемент со свойствами из хранилищ всех отслеживаемых моделей; null - не найден
        BimElementView TryLoadElementView(IModelElementId id);

        // То же по голым Guid'ам (внутренние типы вьюера не реализуют SDK-интерфейсы)
        BimElementView TryLoadElementView(Guid elementId, Guid modelPartId);
    }
}
