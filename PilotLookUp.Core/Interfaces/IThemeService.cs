namespace PilotLookUp.Core.Interfaces
{
    public interface IThemeService
    {
        ThemeNames CurrentTheme { get; set; }
        
        enum ThemeNames
        {
            Jedi,
            Dark
        }
    }
} 