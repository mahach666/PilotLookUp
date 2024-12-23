using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PilotLookUp.Objects
{
    public class ObjectSet : List<PilotObjectHelper>
    {
        public ObjectSet()
        {
        }
        public ObjectSet(IEnumerable<PilotObjectHelper> collection)
        {
            foreach (PilotObjectHelper item in collection)
            {
                Add(item);
            }
        }

        public string Discription
        {
            get
            {
                if (Count == 0) return "No objects";
                else if (Count == 1) return this.FirstOrDefault()?.Name ?? "null";
                else return $"List<{this.FirstOrDefault()?.LookUpObject.GetType().Name}>Count = {Count}";
            }
        }

        public override string ToString()
        {
            return Discription;
        }
    }
}
