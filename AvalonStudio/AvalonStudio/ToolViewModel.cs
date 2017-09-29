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

            CloseCommand = ReactiveCommand.Create(() =>
            {
                // Workspace.Instance.Tools.Remove (this);
            });
        }

        public ReactiveCommand CloseCommand { get; }

        public string Name { get; private set; }
    }
}