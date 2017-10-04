using Avalonia.Media;
using AvalonStudio.Extensibility.Plugin;
using System.Collections;
using System.Collections.Generic;

namespace AvalonStudio.Extensibility.Theme
{
    public class DefaultColorThemes : IExtension
    {
        public void Activation()
        {
            
        }

        public void BeforeActivation()
        {
        }
    }

    public class ColorTheme
    {
        private static List<ColorTheme> s_themes = new List<ColorTheme>();

        public static IEnumerable<ColorTheme> Themes => s_themes;

        public static void Register (ColorTheme theme)
        {
            s_themes.Add(theme);
        }

        public static readonly ColorTheme VisualStudioLight = new ColorTheme
        {
            Name = "Visual Studio Light",
            WindowBorder = Brush.Parse("#9B9FB9"),
            Background = Brush.Parse("#EEEEF2"),
            Foreground = Brush.Parse("#1E1E1E"),
            ForegroundLight = Brush.Parse("#525252"),
            BorderLight = Brush.Parse("#9B9FB9"),
            BorderMid = Brush.Parse("#9B9FB9"),
            BorderDark = Brush.Parse("#FFCCCEDB"),
            ControlLight = Brush.Parse("#9B9FB9"),
            ControlMid = Brush.Parse("#F5F5F5"),
            ControlDark = Brush.Parse("#E6E7E8")
        };

        public static readonly ColorTheme VisualStudioDark = new ColorTheme
        {
            Name = "Visual Studio Dark",
            WindowBorder = Brush.Parse("#9B9FB9"),
            Background = Brush.Parse("#FF2D2D30"),
            Foreground = Brush.Parse("#FFC4C4C4"),
            ForegroundLight = Brush.Parse("#FF808080"),
            BorderLight = Brush.Parse("#FFAAAAAA"),
            BorderMid = Brush.Parse("#FF888888"),
            BorderDark = Brush.Parse("#FF3E3E42"),
            ControlLight = Brush.Parse("#FFFFFFFF"),
            ControlMid = Brush.Parse("#FF3E3E42"),
            ControlDark = Brush.Parse("#FF252526")
        };

        public string Name { get; private set; }

        public IBrush WindowBorder { get; set; }

        public IBrush Background { get; set; }

        public IBrush Foreground { get; set; }

        public IBrush ForegroundLight { get; set; }

        public IBrush BorderLight { get; set; }

        public IBrush BorderMid { get; set; }

        public IBrush BorderDark { get; set; }

        public IBrush ControlLight { get; set; }

        public IBrush ControlMid { get; set; }

        public IBrush ControlDark { get; set; }
    }
}
