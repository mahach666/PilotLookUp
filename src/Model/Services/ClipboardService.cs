using PilotLookUp.Domain.Interfaces;
using System.Windows;

namespace PilotLookUp.Model.Services
{
    public class ClipboardService : IClipboardService
    {
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
                // Логируем ошибку, но не показываем пользователю
                System.Diagnostics.Debug.WriteLine($"Ошибка при копировании в буфер обмена: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine($"Ошибка при получении текста из буфера обмена: {ex.Message}");
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
                System.Diagnostics.Debug.WriteLine($"Ошибка при проверке наличия текста в буфере обмена: {ex.Message}");
                return false;
            }
        }
    }
} 