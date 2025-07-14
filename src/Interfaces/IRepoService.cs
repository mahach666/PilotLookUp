using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Interfaces
{
    public interface IRepoService
    {
        Task<List<ObjectSet>> GetObjInfo(IPilotObjectHelper sender);
        ObjectSet GetWrapedRepo();
        Task<ObjectSet> GetWrapedObjs(IEnumerable<Guid> guids);
    }
}
