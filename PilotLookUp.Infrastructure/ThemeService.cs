namespace PilotLookUp.Infrastructure
{
    public static class ThemeService
    {
        public enum ThemeNames { Jedi, Default }
 
        public static ThemeNames CurrentTheme { get; set; } = ThemeNames.Jedi;
    }
} 