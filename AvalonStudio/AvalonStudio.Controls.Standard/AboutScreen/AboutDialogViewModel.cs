using AvalonStudio.Extensibility.Dialogs;
using AvalonStudio.Platforms;
using ReactiveUI;
using System;
using System.Diagnostics;
using System.Reactive;
using System.Reflection;

namespace AvalonStudio.Controls.Standard.AboutScreen
{
    public class AboutDialogViewModel : ModalDialogViewModelBase
    {
        public AboutDialogViewModel() : base("About", true, false)
        {
            OKCommand = ReactiveCommand.Create(()=>Close());

            PlatformString = Platform.OSDescription + " " + Platform.AvalonRID;
        }

        public override ReactiveCommand<Unit, Unit> OKCommand { get; protected set; }

        public string Version => FileVersionInfo.GetVersionInfo(Assembly.GetEntryAssembly().Location).FileVersion;
	public string DescriptiveVersion => ThisAssembly.Git.Tag + (ThisAssembly.Git.IsDirty ? "-dirty": "");

        public string PlatformString { get; }
    }
}
