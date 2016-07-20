using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace AvalonStudio.Controls
{
	public class Editor : UserControl
	{
		private readonly CompositeDisposable disposables;
		private TextEditor.TextEditor editor;
		private EditorViewModel editorViewModel;

		public Editor()
		{
			InitializeComponent();

			disposables = new CompositeDisposable();
			editor = this.Find<TextEditor.TextEditor>("editor");

			disposables.Add(DataContextProperty.Changed.Subscribe(o =>
			{
				if (o.NewValue is EditorViewModel) // for some reason intellisense view model gets passed here! bug in avalonia?
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

		~Editor()
		{
			Console.WriteLine("Editor UserControl Destructed.");
		}

		protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
		{
			editor = this.Find<TextEditor.TextEditor>("editor");
		}

		protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
		{
			editor = null;
			editorViewModel = null;

			disposables.Dispose();
		}


		private void InitializeComponent()
		{
			AvaloniaXamlLoader.Load(this);
		}
	}
}