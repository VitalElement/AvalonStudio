using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Shell;
using System;
using System.Collections.Generic;
using System.Text;

namespace AvalonStudio
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            Shell = IoC.Get<IShell>();
            Studio = IoC.Get<IStudio>();
        }

        public IShell Shell { get; }

        public IStudio Studio { get; }
    }
}
