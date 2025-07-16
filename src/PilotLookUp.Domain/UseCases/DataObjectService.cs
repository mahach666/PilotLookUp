using Ascon.Pilot.SDK;
using PilotLookUp.Domain.Entities;
using PilotLookUp.Domain.Interfaces;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PilotLookUp.Domain.UseCases
{
    /// <summary>
    /// Pure domain implementation – работает только с IDataObject из SDK, без WPF.
    /// </summary>
    public class DataObjectService : IDataObjectService
    {
        public IEnumerable<AttrDTO> GetAttrDTOs(IDataObject dataObject)
        {
            if (dataObject == null)
                return Enumerable.Empty<AttrDTO>();

            var objAttr = dataObject.Attributes;
            var typeAttr = dataObject.Type.Attributes;

            var res = typeAttr.Select(attr => new AttrDTO
            {
                Name          = attr.Name,
                Title         = attr.Title,
                Value         = objAttr.TryGetValue(attr.Name, out var value)
                                ? FormatValue(value)
                                : string.Empty,
                IsObligatory  = attr.IsObligatory.ToString(),
                IsService     = attr.IsService.ToString(),
                Type          = attr.Type.ToString(),
                IsInitialized = objAttr.ContainsKey(attr.Name),
                IsValid       = true
            }).ToList();

            // attributes existing on object but absent in type description
            res.AddRange(objAttr.Where(a => !typeAttr.Any(t => t.Name == a.Key))
                                .Select(a => new AttrDTO
                                {
                                    Name         = a.Key,
                                    Title        = "Unknown",
                                    Value        = a.Value?.ToString() ?? string.Empty,
                                    IsObligatory = "Unknown",
                                    IsService    = "Unknown",
                                    Type         = "Unknown",
                                    IsInitialized = true,
                                    IsValid       = false
                                }));
            return res;
        }

        private static string FormatValue(object value)
        {
            if (value is string s)
                return s;
            if (value is IEnumerable enumerable)
                return string.Join("; ", enumerable.Cast<object>().Select(item => item?.ToString() ?? string.Empty));
            return value?.ToString() ?? string.Empty;
        }
    }
} 