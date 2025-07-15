using Ascon.Pilot.Themes;

namespace PilotLookUp.Domain.Interfaces
{
    public interface IThemeProvider
    {
        ThemeNames Theme { get; }
    }
} 