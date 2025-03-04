using PilotLookUp.Enums;
using PilotLookUp.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Contracts
{
    public class StartViewInfo
    {
        public PagesName PageName{ get; set; }
        public ObjectSet SelectedObject { get; set; }
    }
}
