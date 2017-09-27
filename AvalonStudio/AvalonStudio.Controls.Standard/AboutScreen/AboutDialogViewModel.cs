using AvalonStudio.Extensibility.Dialogs;
using ReactiveUI;
using System;

namespace AvalonStudio.Controls.Standard.AboutScreen
{
    public class AboutDialogViewModel : ModalDialogViewModelBase
    {
        public AboutDialogViewModel() : base("About", true, false)
        {
            OKCommand = ReactiveCommand.Create(()=>Close());
        }

        public override ReactiveCommand OKCommand { get; protected set; }
    }
}