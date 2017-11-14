using Avalonia.Controls;
using Avalonia.LogicalTree;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Extensibility.Editor
{
    public class EditorView : UserControl
    {
        public EditorView()
        {
            InitializeComponent();
        }

        ~EditorView ()
        {
            System.Console.WriteLine("EditorView Disposed");
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnAttachedToLogicalTree(LogicalTreeAttachmentEventArgs e)
        {
            base.OnAttachedToLogicalTree(e);

            /* _editor = this.FindControl<Standard.CodeEditor.CodeEditor>("editor");

             _editor.RequestTooltipContent += _editor_RequestTooltipContent;

             shell = IoC.Get<IShell>();

             _editor.GetObservable(AvalonStudio.Controls.Standard.CodeEditor.CodeEditor.IsDirtyProperty).Subscribe(dirty =>
             {
                 if (dirty && DataContext is EditorViewModel editorVm)
                 {
                     editorVm.Dock = Dock.Left;

                     // Selecting the document event though it already is, causes it to be removed from the temporary document cache.
                     editorVm.IsTemporary = false;
                     shell.SelectedDocument = editorVm;
                 }
             });

             _editor.AddHandler(PointerWheelChangedEvent, (sender, ee) => { _editorViewModel?.OnPointerWheelChanged(ee); }, Avalonia.Interactivity.RoutingStrategies.Tunnel, handledEventsToo: true);*/
        }

        /* private void _editor_RequestTooltipContent(object sender, Standard.TooltipDataRequestEventArgs e)
         {
             if (DataContext != null)
             {
                 var editorVm = DataContext as EditorViewModel;

                 e.GetViewModelAsyncTask = editorVm.UpdateToolTipAsync;
             }
         }*/
    }
}