using Ascon.Pilot.Themes;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Model.Services
{
    public class ThemeService : IThemeService
    {
        private readonly ThemeNames _theme;

        public ThemeService(ThemeNames theme)
        {
            _theme = theme;
        }

        public ThemeNames CurrentTheme => _theme;
        public bool IsJediTheme => _theme == ThemeNames.Jedi;
    }
} 