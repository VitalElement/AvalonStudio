using ReactiveUI;
using System;

namespace AvalonStudio
{
    public abstract class OldToolViewModel : PaneViewModel
    {
        public OldToolViewModel(string contentId, string name) : base(contentId)
        {
            Name = name;
            ToolTitle = name;

            CloseCommand = ReactiveCommand.Create();
            CloseCommand.Subscribe(o =>
            {
                // Workspace.Instance.Tools.Remove (this);
            });
        }

        public ReactiveCommand<object> CloseCommand { get; }

        public string Name { get; private set; }
    }
}