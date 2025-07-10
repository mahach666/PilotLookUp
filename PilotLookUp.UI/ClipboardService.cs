using System.Windows;
using PilotLookUp.Interfaces;

namespace PilotLookUp.UI
{
    public class ClipboardService : IClipboardService
    {
        public string GetText() => Clipboard.GetText();
        public void SetText(string text) => Clipboard.SetText(text);
    }
} 