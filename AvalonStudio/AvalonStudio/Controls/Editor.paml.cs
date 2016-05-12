namespace AvalonStudio.Controls
{
    using AvalonStudio.TextEditor;
    using Avalonia;
    using Avalonia.Controls;
    using System.Reactive.Disposables;
    using System;

    public class Editor : UserControl
    {
        private TextEditor editor;
        private EditorViewModel editorViewModel;
        private CompositeDisposable disposables;

        ~Editor()
        {
            System.Console.WriteLine(("Editor UserControl Destructed."));
        }

        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {            
            editor = this.Find<TextEditor>("editor");
        }

        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            editor = null;
            editorViewModel = null;

            disposables.Dispose();
        }

        public Editor()
        {                        
            InitializeComponent();

            disposables = new CompositeDisposable();
            editor = this.Find<TextEditor>("editor");

            disposables.Add(DataContextProperty.Changed.Subscribe((o) =>
            {
                if (o.NewValue is EditorViewModel)  // for some reason intellisense view model gets passed here! bug in avalonia?
                {
                    if (o.OldValue is EditorViewModel && (o.OldValue as EditorViewModel).Model.Editor == editor)
                    {
                        (o.OldValue as EditorViewModel).Model.Editor = null;
                    }

                    if (editorViewModel != DataContext)
                    {
                        editorViewModel = DataContext as EditorViewModel;

                        if (editorViewModel != null && editor != null)
                        {
                            editorViewModel.Model.Editor = editor;
                            editor.Focus();
                        }
                    }
                }
            }));
        }


        private void InitializeComponent()
        {
            this.LoadFromXaml();

        }
    }
}
