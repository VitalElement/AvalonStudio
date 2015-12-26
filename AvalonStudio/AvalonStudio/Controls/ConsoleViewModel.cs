namespace AvalonStudio.Controls
{
    using System;
    using Perspex.Threading;
    using Models;
    using ReactiveUI;
    using TextEditor.Document;
    using ReactiveUI;
    using Utils;

    public class ConsoleViewModel : ReactiveObject, IConsole
    {
        public ConsoleViewModel()
        {
            Document = new TextDocument();
        }


        public TextDocument Document { get; private set; }

        private int caretIndex;
        public int CaretIndex
        {
            get { return caretIndex; }
            set { this.RaiseAndSetIfChanged(ref caretIndex, value); }
        }

        private void ScrollToEnd ()
        {
            CaretIndex = Document.TextLength;
        }

        public void Clear()
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                Document.Text = string.Empty;                
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
