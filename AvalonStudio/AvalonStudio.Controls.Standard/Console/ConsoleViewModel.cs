using Avalonia.Threading;
using AvaloniaEdit.Document;
using AvaloniaEdit.Rendering;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
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

        public ConsoleViewModel()
        {
            Title = "Console";
            document = new TextDocument();
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
                Document = new TextDocument();

                IsSelected = true;
            });
        }

        public void Write(char data)
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Document.Insert(Document.TextLength, data.ToString());
                ScrollToEnd();
            });
        }

        public void Write(string data)
        {
            if (data != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Document.Insert(Document.TextLength, data.Replace("\t", "    "));
                    ScrollToEnd();
                });
            }
        }

        public void WriteLine()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Document.Insert(Document.TextLength, Environment.NewLine);
                ScrollToEnd();
            });
        }

        public void WriteLine(string data)
        {
            if (data != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Document.Insert(Document.TextLength, data.Replace("\t", "    ") + Environment.NewLine);
                    ScrollToEnd();
                });
            }
        }

        public void OverWrite(string data)
        {
            WriteLine(data);
        }

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
            shell = IoC.Get<IShell>();
        }

        private void ScrollToEnd()
        {
            CaretIndex = Document.TextLength;
        }
    }
}