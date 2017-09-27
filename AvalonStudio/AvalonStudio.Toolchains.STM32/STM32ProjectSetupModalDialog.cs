namespace AvalonStudio.Toolchains.STM32
{
    using AvalonStudio.Extensibility.Dialogs;
    using ReactiveUI;
    using System;

    public class STM32ProjectSetupModalDialogViewModel : ModalDialogViewModelBase
    {
        public STM32ProjectSetupModalDialogViewModel() : base("New STM32 Project", true, true)
        {
            OKCommand = ReactiveCommand.Create(()=>Close());
        }
    }
}