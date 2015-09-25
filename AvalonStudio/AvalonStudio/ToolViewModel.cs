using Perspex.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using AvalonStudio.Controls.ViewModel;
using AvalonStudio.MVVM;

namespace AvalonStudio
{
    public abstract class ToolViewModel : PaneViewModel
    {
        public ToolViewModel (string contentId, string name) : base (contentId)
        {
            Name = name;
            ToolTitle = name;
            
            this.CloseCommand = new RoutedCommand ((o) =>
            {
               // Workspace.This.Tools.Remove (this);
            });
        }

        public ICommand CloseCommand { get; private set; }

        public string Name
        {
            get;
            private set;
        }
    }
}
