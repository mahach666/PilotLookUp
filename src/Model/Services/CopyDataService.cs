using PilotLookUp.Interfaces;
using PilotLookUp.Objects;
using PilotLookUp.ViewModel;
using PilotLookUp.Contracts;

namespace PilotLookUp.Model.Services
{
    public class CopyDataService : ICopyDataService
    {
        private readonly IClipboardService _clipboardService;
        private readonly IUserNotificationService _notificationService;

        public CopyDataService(IClipboardService clipboardService, IUserNotificationService notificationService)
        {
            _clipboardService = clipboardService;
            _notificationService = notificationService;
        }

        public void CopyObjectName(ListItemVM dataObject)
        {
            if (dataObject?.PilotObjectHelper?.Name == null)
            {
                _notificationService.ShowError("Упс, ничего не выбрано.");
                return;
            }

            _clipboardService.CopyToClipboard(dataObject.PilotObjectHelper.Name);
        }

        public void CopyMemberName(ObjectSet dataGridSelected)
        {
            if (dataGridSelected?.SenderMemberName == null)
            {
                _notificationService.ShowError("Упс, ничего не выбрано.");
                return;
            }

            _clipboardService.CopyToClipboard(dataGridSelected.SenderMemberName);
        }

        public void CopyMemberValue(ObjectSet dataGridSelected)
        {
            if (dataGridSelected?.Discription == null)
            {
                _notificationService.ShowError("Упс, ничего не выбрано.");
                return;
            }

            _clipboardService.CopyToClipboard(dataGridSelected.Discription);
        }

        public void CopyMemberLine(ObjectSet dataGridSelected)
        {
            if (dataGridSelected?.SenderMemberName == null)
            {
                _notificationService.ShowError("Упс, ничего не выбрано.");
                return;
            }

            var line = $"{dataGridSelected.SenderMemberName}\t{dataGridSelected.Discription}";
            _clipboardService.CopyToClipboard(line);
        }

        public void CopyAttributeName(object attribute)
        {
            if (attribute is AttrDTO attrDTO)
            {
                if (attrDTO.Name == null)
                {
                    _notificationService.ShowError("Упс, ничего не выбрано.");
                    return;
                }
                _clipboardService.CopyToClipboard(attrDTO.Name);
            }
        }

        public void CopyAttributeValue(object attribute)
        {
            if (attribute is AttrDTO attrDTO)
            {
                if (attrDTO.Value == null)
                {
                    _notificationService.ShowError("Упс, ничего не выбрано.");
                    return;
                }
                _clipboardService.CopyToClipboard(attrDTO.Value);
            }
        }

        public void CopyAttributeTitle(object attribute)
        {
            if (attribute is AttrDTO attrDTO)
            {
                if (attrDTO.Title == null)
                {
                    _notificationService.ShowError("Упс, ничего не выбрано.");
                    return;
                }
                _clipboardService.CopyToClipboard(attrDTO.Title);
            }
        }
    }
} 