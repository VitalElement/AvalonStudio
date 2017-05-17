using AvaloniaEdit.Document;
using AvalonStudio.Extensibility;
using AvalonStudio.Languages;
using AvalonStudio.Shell;
using System;
using System.Composition;
using System.Reactive.Subjects;

namespace AvalonStudio.Controls
{
    [Export(typeof(EditorModel))]
    public class EditorModel : IDisposable
    {
        

        private readonly IShell shell;

        public EditorModel()
        {
            shell = IoC.Get<IShell>();

            TextDocument = new TextDocument();
        }

        public AvaloniaEdit.TextEditor Editor { get; set; }

        

        public TextDocument TextDocument { get; set; }
        public string Title { get; set; }

        

        

        public void Dispose()
        {
            Editor = null;
        }

        ~EditorModel()
        {
        }

        public void ScrollToLine(int line)
        {
            Editor?.ScrollToLine(line);
        }

        public event EventHandler<EventArgs> DocumentLoaded;

        public event EventHandler<EventArgs> TextChanged;


        

        public void OnBeforeTextChanged(object param)
        {
        }

        private Subject<bool> analysisTriggerEvents = new Subject<bool>();
    }
}