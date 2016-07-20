namespace AvalonStudio.Shell.Commands
{
	using System;
	using System.ComponentModel.Composition;
	using Avalonia.Input;
	using Controls;
	using Extensibility;
	using Extensibility.Commands;
	using ReactiveUI;
	[CommandDefinition]
	public class PackagesCommandDefinition : CommandDefinition
	{
		public PackagesCommandDefinition()
		{
			_command = ReactiveCommand.Create();

			_command.Subscribe(_ =>
			{
				IShell shell = IoC.Get<IShell>();
				shell.ModalDialog = new PackageManagerDialogViewModel();
				shell.ModalDialog.ShowDialog();
			});
		}

		public override string Text => "Packages";

		public override string ToolTip => "Packages Tool Tip";

		readonly ReactiveCommand<object> _command;
		public override System.Windows.Input.ICommand Command => _command;

		[Export]
		public static CommandKeyboardShortcut KeyGesture = new CommandKeyboardShortcut<ExitCommandDefinition>(new KeyGesture() { Key = Key.F4, Modifiers = InputModifiers.Alt });
	}
}