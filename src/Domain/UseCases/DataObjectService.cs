using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Domain.UseCases
{
    public class DataObjectService : IDataObjectService
    {
        public IEnumerable<AttrDTO> GetAttrDTOs(IPilotObjectHelper dataObjectHelper)
        {
            var castedObject = dataObjectHelper.LookUpObject as IDataObject;

            var objAttr = castedObject.Attributes;
            var typeAttr = castedObject.Type.Attributes;

            var res = typeAttr.Select(attr => new AttrDTO
            {
                Name = attr.Name,
                Title = attr.Title,
                Value = objAttr.TryGetValue(attr.Name, out var value) ? value?.ToString() : string.Empty,
                IsObligatory = attr.IsObligatory.ToString(),
                IsService = attr.IsService.ToString(),
                Type = attr.Type.ToString(),
                IsInitialized = objAttr.ContainsKey(attr.Name),
                IsValid = true
            }).ToList();

            res.AddRange(objAttr.Where(attr => !typeAttr.Any(typeAttrItem => typeAttrItem.Name == attr.Key))
                .Select(attr => new AttrDTO
                {
                    Name = attr.Key,
                    Title = PilotLookUp.Resources.Strings.Unknown,
                    Value = attr.Value?.ToString() ?? string.Empty,
                    IsObligatory = PilotLookUp.Resources.Strings.Unknown,
                    IsService = PilotLookUp.Resources.Strings.Unknown,
                    Type = PilotLookUp.Resources.Strings.Unknown,
                    IsValid = false
                }));

            return res;
        }
    }
}
