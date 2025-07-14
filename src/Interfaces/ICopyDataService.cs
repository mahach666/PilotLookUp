using PilotLookUp.Objects;
using PilotLookUp.ViewModel;

namespace PilotLookUp.Interfaces
{
    public interface ICopyDataService
    {
        void CopyObjectName(ListItemVM dataObject);
        void CopyMemberName(ObjectSet dataGridSelected);
        void CopyMemberValue(ObjectSet dataGridSelected);
        void CopyMemberLine(ObjectSet dataGridSelected);
        void CopyAttributeName(object attribute);
        void CopyAttributeValue(object attribute);
        void CopyAttributeTitle(object attribute);
    }
} 