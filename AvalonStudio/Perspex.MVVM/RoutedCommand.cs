using System;
using System.Windows.Input;

namespace Perspex.MVVM
{
    public class OpacityCommand : RoutedCommand<object>
    {
        public OpacityCommand(Action<object> command, Predicate<object> predicate)
            : base(command, predicate)
        {
            this.CanExecuteChanged += OpacityCommand_CanExecuteChanged;
        }

        void OpacityCommand_CanExecuteChanged(object sender, EventArgs e)
        {
            if (this.CanExecute(null))
            {
                Opacity = 1.0;
            }
            else
            {
                Opacity = 0.2;
            }
        }

        private double opacity;
        public double Opacity
        {
            get { return opacity; }
            set
            {
                if (value != opacity)
                { opacity = value; OnPropertyChanged(); }
            }
        }
    }

    /// <summary>
    /// A command implementation. Being bound to a view control, routes commands to the given delegates
    /// taking one parameter of type T.
    /// </summary>
    /// <typeparam name="T">is the type of the command parameter.</typeparam>
    public class RoutedCommand<T> : ViewModelBase, ICommand
    {
        #region Construction and destruction
        /// <summary>
        /// Initializes a new instance of the <see cref="{T}" /> class.
        /// </summary>
        /// <param name="command">The Action that will be called when the command is executed.</param>
        /// <param name="predicate">Predicate to evaluate if the command can be called.</param>
        public RoutedCommand(Action<T> command, Predicate<T> predicate)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }

            this._command = command;
            this._predicate = predicate;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="{T}" /> class.
        /// </summary>
        /// <param name="command">The Action that will be called when the command is executed.</param>
        public RoutedCommand(Action<T> command)
            : this(command, null)
        {
        }
        #endregion

        #region Public members
        /// <summary>
        /// Event to be triggered whenever CanExecute has changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (this._predicate != null)
                {
                    // CommandManager.RequerySuggested += value;
                }
            }

            remove
            {
                if (this._predicate != null)
                {
                    //CommandManager.RequerySuggested -= value;
                }
            }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Any paramter that the command uses.</param>
        public void Execute(object parameter)
        {
            this._command((T)parameter);
        }

        /// <summary>
        /// Evaluates if the command can execute.
        /// </summary>
        /// <param name="parameter">Any parameter that the command uses.</param>
        /// <returns>true or false indicating the result.</returns>
        public bool CanExecute(object parameter)
        {
            if (this._predicate != null)
            {
                return this._predicate((T)parameter);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Triggers an invalidate command.
        /// </summary>
        /// <param name="sender">object that emitted the event.</param>
        /// <param name="e">Event Args.</param>
        public void Update(object sender, EventArgs e)
        {
            //CommandManager.InvalidateRequerySuggested();
        }
        #endregion

        #region Private variables
        /// <summary>
        /// Action that gets called when the command executes.
        /// </summary>
        private Action<T> _command;

        /// <summary>
        /// Predicate that gets called in order to evaluate if the command can be executed.
        /// </summary>
        private Predicate<T> _predicate;
        #endregion
    }

    /// <summary>
    /// A command implementation. Being bound to a view control, routes commands to the given delegates
    /// taking one parameter of type object.
    /// </summary>
    public sealed class RoutedCommand : RoutedCommand<object>
    {
        private ICommand editScatterFileCommand;
        #region Construction and destruction
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedCommand"/> class.
        /// </summary>
        /// <param name="command">The Action that will be executed when the command is executed.</param>
        /// <param name="predicate">The predicate to evaluate if the command can be called.</param>
        public RoutedCommand(Action<object> command, Predicate<object> predicate)
            : base(command, predicate)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutedCommand"/> class.
        /// </summary>
        /// <param name="command">The Action that will be executed when the command is executed.</param>
        public RoutedCommand(Action<object> command)
            : this(command, null)
        {
        }
        #endregion
    }
}
