using System;
using Avalonia.Controls.Shapes;
using ReactiveUI;

namespace AvalonStudio.Extensibility.Commands
{
	public class Command : ReactiveObject
	{
		private bool _checked;
		private bool _enabled = true;
		private Uri _iconSource;
		private string _text;
		private string _toolTip;
		private bool _visible = true;

		private Path iconPath;

		public Command(CommandDefinitionBase commandDefinition)
		{
			CommandDefinition = commandDefinition;
			Text = commandDefinition.Text;
			ToolTip = commandDefinition.ToolTip;
			//IconSource = commandDefinition.IconSource;
		}

		public CommandDefinitionBase CommandDefinition { get; }

		public bool Visible
		{
			get { return _visible; }
			set { this.RaiseAndSetIfChanged(ref _visible, value); }
		}

		public bool Enabled
		{
			get { return _enabled; }
			set { this.RaiseAndSetIfChanged(ref _enabled, value); }
		}

		public bool Checked
		{
			get { return _checked; }
			set { this.RaiseAndSetIfChanged(ref _checked, value); }
		}

		public string Text
		{
			get { return _text; }
			set { this.RaiseAndSetIfChanged(ref _text, value); }
		}

		public string ToolTip
		{
			get { return _toolTip; }
			set { this.RaiseAndSetIfChanged(ref _toolTip, value); }
		}

		public Uri IconSource
		{
			get { return _iconSource; }
			set { this.RaiseAndSetIfChanged(ref _iconSource, value); }
		}

		public Path IconPath
		{
			get { return iconPath; }
			set { this.RaiseAndSetIfChanged(ref iconPath, value); }
		}

		public object Tag { get; set; }
	}
}