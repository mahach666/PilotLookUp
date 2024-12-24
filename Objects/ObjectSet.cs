using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
                else if (Count == 1) return this.FirstOrDefault()?.Name ?? "NULL";
                else return $"List<{this.FirstOrDefault()?.LookUpObject?.GetType().Name: invalid}>Count = {Count}";
            }
        }

        public Brush Color
        {
            get
            {
                if (IsLookable == true)
                {
                    return new SolidColorBrush(Colors.Blue);
                }
                return new SolidColorBrush(Colors.Black);
            }
        }

        public TextDecorationCollection Decoration
        {
            get
            {
                if (IsLookable == true)
                {
                    return TextDecorations.Underline;
                }
                return null;
            }
        }

        public bool IsLookable 
        {
            get
            {
                if (this.FirstOrDefault()?.IsLookable == true)
                {
                    return true;
                }
                return false;
            }
        }

        public override string ToString()
        {
            return Discription;
        }
    }
}
