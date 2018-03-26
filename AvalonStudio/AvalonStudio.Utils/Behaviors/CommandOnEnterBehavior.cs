using Avalonia;
using Avalonia.Controls;
using Avalonia.Xaml.Interactivity;
using System.Reactive.Disposables;
using System.Windows.Input;

namespace AvalonStudio.Utils.Behaviors
{
    public class CommandOnEnterBehavior : Behavior<TextBox>
    {
        private CompositeDisposable _disposables;
        private ICommand _command;

        protected override void OnAttached()
        {
            _disposables = new CompositeDisposable();

            base.OnAttached();

            _disposables.Add(AssociatedObject.AddHandler(TextBox.KeyDownEvent, (sender, e) => 
            {
                if(e.Key == Avalonia.Input.Key.Enter)
                {
                    if (Command != null)
                    {
                        Command.Execute(CommandParameter);
                        e.Handled = true;
                    }
                }
            }));

        }

        /// <summary>
        /// Defines the <see cref="Command"/> property.
        /// </summary>
        public static readonly DirectProperty<CommandOnEnterBehavior, ICommand> CommandProperty =
            AvaloniaProperty.RegisterDirect<CommandOnEnterBehavior, ICommand>(nameof(Command),
                button => button.Command, (button, command) => button.Command = command, enableDataValidation: true);        

        /// <summary>
        /// Defines the <see cref="CommandParameter"/> property.
        /// </summary>
        public static readonly StyledProperty<object> CommandParameterProperty =
            AvaloniaProperty.Register<CommandOnEnterBehavior, object>(nameof(CommandParameter));

        /// <summary>
        /// Gets or sets an <see cref="ICommand"/> to be invoked when the button is clicked.
        /// </summary>
        public ICommand Command
        {
            get { return _command; }
            set { SetAndRaise(CommandProperty, ref _command, value); }
        }

        /// <summary>
        /// Gets or sets a parameter to be passed to the <see cref="Command"/>.
        /// </summary>
        public object CommandParameter
        {
            get { return GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            _disposables.Dispose();
        }
    }
}
