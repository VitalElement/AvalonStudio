using System;

using Perspex;
using Perspex.Controls;
using Perspex.Diagnostics;
using Perspex.Themes.Default;
using AvalonStudio.TextEditor;
using AvalonStudio.Controls;

namespace AvalonStudio
{
    class App : Application
    {
        public App()
        {
            RegisterServices();
            InitializeSubsystems((int)Environment.OSVersion.Platform);
            Styles = new DefaultTheme();
            Styles.Add(new TextEditorTheme());
            Styles.Add(new MetroWindowTheme());
        }

        public static void AttachDevTools(Window window)
        {
#if DEBUG
            DevTools.Attach(window);
#endif
        }
    }
}
