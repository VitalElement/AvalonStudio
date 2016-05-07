namespace AvalonStudio.Controls.Standard.Console
{
    using System;
    using Perspex.Threading;
    using ReactiveUI;
    using TextEditor.Document;
    using Utils;
    using TextEditor.Rendering;
    using System.Collections.ObjectModel;
    using MVVM;
    using Extensibility.Plugin;
    using Extensibility;
    using Shell;

    public class ConsoleViewModel : ToolViewModel, IConsole, IPlugin
    {
        private IShell shell;

        public ConsoleViewModel()
        {
            Title = "Console";
            Document = new TextDocument();
            backgroundRenderers = new ObservableCollection<IBackgroundRenderer>();
            backgroundRenderers.Add(new SelectedLineBackgroundRenderer());
            backgroundRenderers.Add(new SelectionBackgroundRenderer());
        }

        public void BeforeActivation()
        {
            IoC.RegisterConstant(this, typeof(IConsole)); 
        }

        public void Activation ()
        {
            shell = IoC.Get<IShell>();
        }

        public TextDocument Document { get; private set; }

        private int caretIndex;
        public int CaretIndex
        {
            get { return caretIndex; }
            set { this.RaiseAndSetIfChanged(ref caretIndex, value); }
        }

        private ObservableCollection<IBackgroundRenderer> backgroundRenderers;
        public ObservableCollection<IBackgroundRenderer> BackgroundRenderers
        {
            get { return backgroundRenderers; }
            set { this.RaiseAndSetIfChanged(ref backgroundRenderers, value); }
        }

        public override Location DefaultLocation
        {
            get
            {
                return Location.Bottom;
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public Version Version
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        private void ScrollToEnd()
        {
            CaretIndex = Document.TextLength;
        }

        public void Clear()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                // safe way other than document.text = string.empty
                Document.Replace(0, Document.TextLength, string.Empty);
                
                shell.BottomSelectedTool = this;
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
                    Document.Insert(Document.TextLength, data);
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
                    Document.Insert(Document.TextLength, data + Environment.NewLine);
                    ScrollToEnd();
                });
            }
        }

        public void OverWrite(string data)
        {
            WriteLine(data);
        }
    }
}
