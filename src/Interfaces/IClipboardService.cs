namespace PilotLookUp.Interfaces
{
    public interface IClipboardService
    {
        void CopyToClipboard(string text);
        string GetClipboardText();
        bool HasText();
    }
} 