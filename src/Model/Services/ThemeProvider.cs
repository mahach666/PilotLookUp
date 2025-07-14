using Ascon.Pilot.Themes;
using PilotLookUp.Interfaces;

namespace PilotLookUp.Model.Services
{
    public class ThemeProvider : IThemeProvider
    {
        public ThemeProvider(ThemeNames theme)
        {
            Theme = theme;
        }

        public ThemeNames Theme { get; }
    }
} 