using System;
using System.Collections.ObjectModel;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.TextEditor.Rendering;
using AvalonStudio.Utils;
using ReactiveUI;

namespace AvalonStudio.Controls.Standard.Console
{
	public class ConsoleViewModel : ToolViewModel, IConsole, IPlugin
	{
		private ObservableCollection<IBackgroundRenderer> backgroundRenderers;

		private int caretIndex;
		private IShell shell;

		public ConsoleViewModel()
		{
			Title = "Console";
			Document = new TextDocument();
			backgroundRenderers = new ObservableCollection<IBackgroundRenderer>();
			backgroundRenderers.Add(new SelectedLineBackgroundRenderer());
			backgroundRenderers.Add(new SelectionBackgroundRenderer());
		}

		public TextDocument Document { get; }

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

		public void BeforeActivation()
		{
			IoC.RegisterConstant(this, typeof (IConsole));
		}

		public void Activation()
		{
			shell = IoC.Get<IShell>();
		}

		public string Name
		{
			get { throw new NotImplementedException(); }
		}

		public Version Version
		{
			get { throw new NotImplementedException(); }
		}

		public string Description
		{
			get { throw new NotImplementedException(); }
		}

		private void ScrollToEnd()
		{
			CaretIndex = Document.TextLength;
		}
	}
}