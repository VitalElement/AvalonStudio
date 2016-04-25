namespace AvalonStudio.Controls.Standard.ErrorList
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
                return () => new ErrorList();
            }
        }

        public Type ViewModelType
        {
            get
            {
                return typeof(ErrorListViewModel);
            }
        }
    }

    public class ErrorList : UserControl, ITool<ErrorListViewModel>
    {
        public ErrorList()
        {
            this.InitializeComponent();
        }

        public ErrorListViewModel ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get
            {
                return ViewModel;
            }

            set
            {
                ViewModel = (ErrorListViewModel)value;
            }
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
