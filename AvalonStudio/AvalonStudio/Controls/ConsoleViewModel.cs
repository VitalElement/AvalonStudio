namespace AvalonStudio.Controls
{
    using System;
    using Perspex.Threading;
    using Models;
    using ReactiveUI;
    using Models.Tools;

    public class ConsoleViewModel : ReactiveObject, IConsole
    {        
        private string text;
        public string Text
        {
            get { return text; }
            set { this.RaiseAndSetIfChanged(ref text, value); }
        }

        public void Clear()
        {
			Dispatcher.UIThread.InvokeAsync (() => {
				Text = string.Empty;
			});
        }

        public void Write(char data)
		{
			Dispatcher.UIThread.InvokeAsync (() => {
				Text += data;
			});
        }

        public void Write(string data)
        {
            if (data != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Text += data;
                });
            }
        }

        public void WriteLine()
        {            
			Dispatcher.UIThread.InvokeAsync (() => {
				Text += Environment.NewLine;
			});
        }

        public void WriteLine(string data)
        {
            if (data != null)
            {
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    Text += data + Environment.NewLine;
                });
            }
        }
    }
}
