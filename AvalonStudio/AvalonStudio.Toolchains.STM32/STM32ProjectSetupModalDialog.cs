namespace AvalonStudio.Toolchains.STM32
{
    using AvalonStudio.Extensibility.Dialogs;
    using ReactiveUI;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class STM32ProjectSetupModalDialogViewModel : ModalDialogViewModelBase
    {
        public STM32ProjectSetupModalDialogViewModel() : base("New STM32 Project", true, true)
        {
            OKCommand = ReactiveCommand.Create();

            OKCommand.Subscribe((o) =>
            {
                Close();
            });
        }
    }
}
