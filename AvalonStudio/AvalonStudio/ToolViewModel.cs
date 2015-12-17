namespace AvalonStudio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using AvalonStudio.Controls.ViewModels;
    using AvalonStudio.MVVM;
    using ReactiveUI;

    public abstract class ToolViewModel : PaneViewModel
    {
        public ToolViewModel (string contentId, string name) : base (contentId)
        {
            Name = name;
            ToolTitle = name;

            CloseCommand = ReactiveCommand.Create();
            CloseCommand.Subscribe((o) =>
            {
               // Workspace.Instance.Tools.Remove (this);
            });
        }

        public ReactiveCommand<object> CloseCommand { get; private set; }

        public string Name
        {
            get;
            private set;
        }
    }
}
