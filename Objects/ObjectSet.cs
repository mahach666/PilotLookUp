using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;

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
                else return $"List<{this.FirstOrDefault()?.LookUpObject?.GetType().Name : invalid}>Count = {Count}";
            }
        }

        public Brush Color { get { return new SolidColorBrush(Colors.Red); } }
        
        public override string ToString()
        {
            return Discription;
        }
    }
}
