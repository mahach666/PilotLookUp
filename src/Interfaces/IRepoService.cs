using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PilotLookUp.Interfaces
{
    public interface IRepoService
    {
        public Task<List<ObjectSet>> GetObjInfo(PilotObjectHelper sender);
        public ObjectSet GetWrapedRepo();
        public Task<ObjectSet> GetWrapedObjs(IEnumerable<Guid> guids);
    }
}
