using Ascon.Pilot.Themes;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Model.Services
{
    public class ThemeService : IThemeService
    {
        private readonly IThemeProvider _themeProvider;

        public ThemeService(IThemeProvider themeProvider)
        {
            _themeProvider = themeProvider;
        }

        public ThemeNames CurrentTheme => _themeProvider.Theme;
        public bool IsJediTheme => _themeProvider.Theme == ThemeNames.Jedi;
    }
} 