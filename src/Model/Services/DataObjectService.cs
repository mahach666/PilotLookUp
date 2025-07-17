using Ascon.Pilot.SDK;
using PilotLookUp.Contracts;
using PilotLookUp.Interfaces;
using PilotLookUp.Objects.TypeHelpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Model.Services
{
    public class DataObjectService : IDataObjectService
    {
        public IEnumerable<AttrDTO> GetAttrDTOs(DataObjectHelper dataObjectHelper)
        {
            var castedObject = dataObjectHelper.LookUpObject as IDataObject;

            var objAttr = castedObject.Attributes;
            var typeAttr = castedObject.Type.Attributes;

            var res = typeAttr.Select(attr => new AttrDTO
            {
                Name = attr.Name,
                Title = attr.Title,
                Value = objAttr.TryGetValue(attr.Name, out var value)
                        ? FormatValue(value)
                        : string.Empty,
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
                    Title = "Unknown",
                    Value = attr.Value?.ToString() ?? string.Empty,
                    IsObligatory = "Unknown",
                    IsService = "Unknown",
                    Type = "Unknown",      
                    IsValid = false
                }));

            return res;
        }

        private static string FormatValue(object value)
        {
            if (value is string str)
                return str;

            if (value is IEnumerable enumerable)
            {
                return string.Join("; ", enumerable
                    .Cast<object>()
                    .Select(item => item?.ToString() ?? string.Empty));
            }
            return value?.ToString() ?? string.Empty;
        }
    }
}
