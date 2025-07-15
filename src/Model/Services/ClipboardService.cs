using PilotLookUp.Domain.Interfaces;
using PilotLookUp.Infrastructure;
using System.Windows;

namespace PilotLookUp.Model.Services
{
    public class ClipboardService : IClipboardService
    {

        private readonly ILogger _logger;

        public ClipboardService(ILogger logger)
        {
            _logger = logger;
        }

        public void CopyToClipboard(string text)
        {
            if (string.IsNullOrEmpty(text))
                return;

            try
            {
                Clipboard.SetText(text);
            }
            catch (System.Exception ex)
            {
                _logger.Warn($"Ошибка при копировании в буфер обмена: {ex.Message}");
            }
        }

        public string GetClipboardText()
        {
            try
            {
                return Clipboard.GetText();
            }
            catch (System.Exception ex)
            {
                _logger.Warn($"Ошибка при получении текста из буфера обмена: {ex.Message}");
                return string.Empty;
            }
        }

        public bool HasText()
        {
            try
            {
                return Clipboard.ContainsText();
            }
            catch (System.Exception ex)
            {
                _logger.Warn($"Ошибка при проверке наличия текста в буфере обмена: {ex.Message}");
                return false;
            }
        }
    }
} 