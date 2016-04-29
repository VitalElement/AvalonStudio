namespace AvalonStudio.Controls.Standard.AboutScreen
{
    using Extensibility;
    using Extensibility.Dialogs;
    using ReactiveUI;
    using Shell;
    using System;

    public class AboutDialogViewModel : ModalDialogViewModelBase
    {
        public AboutDialogViewModel() : base("About", true, false)
        {
            OKCommand = ReactiveCommand.Create();
            OKCommand.Subscribe((o) =>
            {   
                Close();
            });                
        }
        
        public override ReactiveCommand<object> OKCommand { get; protected set; }
    }
}
