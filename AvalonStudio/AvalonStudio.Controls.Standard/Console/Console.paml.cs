namespace AvalonStudio.Controls.Standard.Console
{
    using Perspex.Controls;
    using Perspex;
    using AvalonStudio.Extensibility.MVVM;
    using System;
    using ReactiveUI;

    public class ConsoleViewMetaData : IToolMetaData
    {
        public Func<object> Factory
        {
            get
            {
                return () => new Console();
            }
        }

        public Type ViewModelType
        {
            get
            {
                return typeof(ConsoleViewModel);
            }
        }
    }

    public class Console : UserControl, ITool<ConsoleViewModel>
    {
        public Console()
        {
            this.InitializeComponent();            
        }

        public ConsoleViewModel ViewModel
        {
            get;set;
        }

        object IViewFor.ViewModel
        {
            get
            {
                return ViewModel;
            }

            set
            {
                ViewModel = (ConsoleViewModel)value;
            }
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
