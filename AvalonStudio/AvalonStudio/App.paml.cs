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
            InitializeComponent();
        }

        public static void AttachDevTools(Window window)
        {
#if DEBUG
            DevTools.Attach(window);
#endif
        }

        private void InitializeComponent()
        {
            PerspexXamlLoader.Load(this);
        }
    }
}
