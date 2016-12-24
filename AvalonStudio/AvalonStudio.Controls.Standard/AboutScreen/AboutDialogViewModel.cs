using System;
using AvalonStudio.Extensibility.Dialogs;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.AboutScreen
{
	public class AboutDialogViewModel : ModalDialogViewModelBase
	{
		public AboutDialogViewModel() : base("About", true, false)
		{
			OKCommand = ReactiveCommand.Create();
			OKCommand.Subscribe(o => { Close(); });
		}

		public override ReactiveCommand<object> OKCommand { get; protected set; }
	}
}