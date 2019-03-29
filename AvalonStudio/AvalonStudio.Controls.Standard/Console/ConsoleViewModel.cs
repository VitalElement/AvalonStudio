using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Composition;

namespace AvalonStudio.Controls.Standard.Console
{
    [ExportToolControl]
    [Export(typeof(IExtension))]
    [Export(typeof(IConsole))]
    [Shared]
    public class ConsoleViewModel : ToolViewModel, IConsole, IActivatableExtension
    {
        private ObservableCollection<IBackgroundRenderer> backgroundRenderers;

        private int caretIndex;
        private IShell shell;

        public ConsoleViewModel() : base ("Output")
        {            
            document = new TextDocument();
            document.Insert(Document.TextLength, Environment.NewLine);
            document.Insert(Document.TextLength, Environment.NewLine + "     ");
            backgroundRenderers = new ObservableCollection<IBackgroundRenderer>();
        }

        private TextDocument document;

        public TextDocument Document
        {
            get { return document; }
            set { this.RaiseAndSetIfChanged(ref document, value); }
        }

        public int CaretIndex
        {
            get { return caretIndex; }
            set { this.RaiseAndSetIfChanged(ref caretIndex, value); }
        }

        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return backgroundRenderers; }
            set { this.RaiseAndSetIfChanged(ref backgroundRenderers, value); }
        }

        public override Location DefaultLocation
        {
            get { return Location.Bottom; }
        }

        public void Clear()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                document.Text = "";
                document.Insert(Document.TextLength, Environment.NewLine);
                document.Insert(Document.TextLength, Environment.NewLine + "     " + Environment.NewLine);

                IsSelected = true;
            });
        }

        private void Insert(string data)
        {
            Document.Insert(Document.Lines[Document.LineCount - 2].Offset, data);
        }

        private void Overwrite(string data)
        {
            //Document.Insert(Document.Lines[Document.LineCount - 2].Offset, data);

            var line = Document.Lines[Document.LineCount - 3];
            Document.Replace(line, data);
        }

        public void Write(char data)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Insert(data.ToString());
                ScrollToEnd();
            });
        }

        public void Write(string data)
        {
            if (data != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Insert(data.Replace("\t", "    "));
                    ScrollToEnd();
                });
            }
        }

        public void WriteLine()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Insert(Environment.NewLine);
                ScrollToEnd();
            });
        }

        public void WriteLine(string data)
        {
            if (data != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Insert(data.Replace("\t", "    ") + Environment.NewLine);
                    ScrollToEnd();
                });
            }
        }

        public void OverWrite(string data)
        {
            if (data != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Overwrite(data);
                });
            }
        }

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
            shell = IoC.Get<IShell>();

            shell.MainPerspective.AddOrSelectTool(this);
            IoC.Get<IStudio>().DebugPerspective.AddOrSelectTool(this);
        }

        private void ScrollToEnd()
        {
            CaretIndex = Document.TextLength - 1;            
        }
    }
}