using Ascon.Pilot.Themes;

namespace PilotLookUp.Interfaces
{
    public interface IThemeService
    {
        ThemeNames CurrentTheme { get; }
        bool IsJediTheme { get; }
    }
} 