using System;

using Perspex;
using Perspex.Controls;
using Perspex.Diagnostics;
using Perspex.Themes.Default;
using AvalonStudio.TextEditor;
using AvalonStudio.Controls;
using Perspex.Markup.Xaml;
using Perspex.Styling;

namespace AvalonStudio
{
    class App : Application
    {
        public App()
        {            
            RegisterServices();
            InitializeSubsystems((int)Environment.OSVersion.Platform);
            Styles.Add(new DefaultTheme());

            var loader = new PerspexXamlLoader();
            var baseLight = (IStyle)loader.Load(
                new Uri("resm:Perspex.Themes.Default.Accents.BaseLight.paml?assembly=Perspex.Themes.Default"));
            Styles.Add(baseLight);
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
