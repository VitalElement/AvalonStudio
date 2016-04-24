namespace AvalonStudio.Controls.Standard
{
    using System;
    using Perspex;
    using Perspex.Controls;
    using ReactiveUI;
    using ViewModels;
    using Extensibility.MVVM;
    using Extensibility;

    public class SolutionExplorerMetaData : IToolMetaData
    {
        public Func<object> Factory
        {
            get
            {
                return () => new SolutionExplorerView();
            }
        }

        public Type ViewModelType
        {
            get
            {
                return typeof(SolutionExplorerViewModel);
            }
        }
    }

    public class SolutionExplorerView : UserControl, ITool<SolutionExplorerViewModel>
    {
        public SolutionExplorerView()
        {
            this.InitializeComponent();
        }

        public SolutionExplorerViewModel ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            set
            {
                ViewModel = (SolutionExplorerViewModel)value;
            }
            get
            {
                return ViewModel;
            }
        }

        private void InitializeComponent()
        {
            this.LoadFromXaml();
        }
    }
}
