namespace AvalonStudio.Controls
{
    using System;
	using Perspex.Threading;
    using Perspex.MVVM;
    using Models;

    public class ConsoleViewModel : ViewModelBase, IConsole
    {
        public ConsoleViewModel()
        {                        
            //Timer timer = new Timer();
            //timer.Interval = 5000;

            //timer.Tick += (sender, e) =>
            //{
            //    Dispatcher.UIThread.InvokeAsync(() =>
            //    {
            //        Text += "Testing Console: " + DateTime.Now + " " + Environment.NewLine;
            //    });
            //};

            //timer.Start();
        }

        private string text;
        public string Text
        {
            get { return text; }
            set { text = value; OnPropertyChanged(); }
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
			Dispatcher.UIThread.InvokeAsync (() => {
				Text += data;
			});
        }

        public void WriteLine()
        {
			Dispatcher.UIThread.InvokeAsync (() => {
				Text += Environment.NewLine;
			});
        }

        public void WriteLine(string data)
        {
			Dispatcher.UIThread.InvokeAsync (() => {
				Text += data + Environment.NewLine;
			});
        }
    }
}
