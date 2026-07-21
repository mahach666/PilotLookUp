using System;
using System.Collections.Generic;

namespace PilotLookUp.Objects
{
    // Событие на стороне клиента (выделение, клики меню плагина)
    public sealed class ClientEventRecord
    {
        public ClientEventRecord(string name, string message, IReadOnlyList<Guid> objectIds = null, string typeInfo = null)
        {
            Name = name;
            Message = message;
            ObjectIds = objectIds ?? Array.Empty<Guid>();
            TypeInfo = typeInfo ?? string.Empty;
            DateTime = DateTime.Now;
        }

        public string Name { get; }
        public string Message { get; }
        public IReadOnlyList<Guid> ObjectIds { get; }
        public string TypeInfo { get; }
        public DateTime DateTime { get; }
    }
}
