namespace AvalonStudio.Extensibility.Commands
{
    using System;
    using ReactiveUI;

    public class Command : ReactiveObject
    {
        private readonly CommandDefinitionBase _commandDefinition;
        private bool _visible = true;
        private bool _enabled = true;
        private bool _checked;
        private string _text;
        private string _toolTip;
        private Uri _iconSource;

        public CommandDefinitionBase CommandDefinition
        {
            get { return _commandDefinition; }
        }

        public bool Visible
        {
            get { return _visible; }
            set
            {
                this.RaiseAndSetIfChanged(ref _visible, value);
            }
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                this.RaiseAndSetIfChanged(ref _enabled, value);
            }
        }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                this.RaiseAndSetIfChanged(ref _checked, value);
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                this.RaiseAndSetIfChanged(ref _text, value);
            }
        }

        public string ToolTip
        {
            get { return _toolTip; }
            set
            {
                this.RaiseAndSetIfChanged(ref _toolTip, value);
            }
        }

        public Uri IconSource
        {
            get { return _iconSource; }
            set
            {
                this.RaiseAndSetIfChanged(ref _iconSource, value);
            }
        }

        public object Tag { get; set; }

        public Command(CommandDefinitionBase commandDefinition)
        {
            _commandDefinition = commandDefinition;
            Text = commandDefinition.Text;
            ToolTip = commandDefinition.ToolTip;
            //IconSource = commandDefinition.IconSource;
        }
    }
}