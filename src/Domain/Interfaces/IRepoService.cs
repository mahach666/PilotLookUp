using PilotLookUp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PilotLookUp.Domain.Interfaces;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IRepoService
    {
        Task<List<ObjectSet>> GetObjInfo(IPilotObjectHelper sender);
        ObjectSet GetWrapedRepo();
        Task<ObjectSet> GetWrapedObjs(IEnumerable<Guid> guids);
    }
}
