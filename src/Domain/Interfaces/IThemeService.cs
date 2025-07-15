using Ascon.Pilot.Themes;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IThemeService
    {
        ThemeNames CurrentTheme { get; }
        bool IsJediTheme { get; }
    }
} 