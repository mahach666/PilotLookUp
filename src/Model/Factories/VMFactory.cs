using PilotLookUp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Model.Factories
{
    public class VMFactory
    {
        private readonly Func< LookUpVM> _lookUpVMFactory;
        private readonly Func<SearchVM> _searchVMFactory;

        public VMFactory(Func<LookUpVM> lookUpVMFactory,
            Func<SearchVM> searchVMFactory)
        {
            _lookUpVMFactory = lookUpVMFactory;
            _searchVMFactory = searchVMFactory;
        }

        public LookUpVM CreateLookUpVM()
        {
            return _lookUpVMFactory();
        }
        public SearchVM CreateSearchVM()
        {
            return _searchVMFactory();
        }
    }
}
