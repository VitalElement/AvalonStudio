using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Languages;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.TextEditor.Document;
using AvalonStudio.Extensibility.Languages.CompletionAssistance;
using AvalonStudio.Utils;

namespace AvalonStudio.Controls
{
	[Export(typeof (EditorModel))]
	public class EditorModel : IDisposable
	{
		public static List<UnsavedFile> UnsavedFiles = new List<UnsavedFile>();

		private CancellationTokenSource cancellationSource;

		private CodeAnalysisResults codeAnalysisResults;
		private readonly JobRunner codeAnalysisRunner;

		private readonly IShell shell;

		public EditorModel()
		{
			shell = IoC.Get<IShell>();

			codeAnalysisRunner = new JobRunner();
			TextDocument = new TextDocument();
		}

		public TextEditor.TextEditor Editor { get; set; }

		public ISourceFile ProjectFile { get; private set; }

		public TextDocument TextDocument { get; set; }
		public string Title { get; set; }

		public CodeAnalysisResults CodeAnalysisResults
		{
			get { return codeAnalysisResults; }
			set
			{
				codeAnalysisResults = value;

				if (CodeAnalysisCompleted != null)
				{
					CodeAnalysisCompleted(this, new EventArgs());
				}
			}
		}


		public ILanguageService LanguageService { get; set; }

		public bool IsDirty { get; set; }


		public void Dispose()
		{
			Editor = null;
			TextDocument.TextChanged -= TextDocument_TextChanged;
		}

		~EditorModel()
		{
		}

		public async Task<CodeCompletionResults> DoCompletionRequestAsync(int line, int column)
		{
			CodeCompletionResults results = null;

			var completions = await LanguageService.CodeCompleteAtAsync(ProjectFile, line, column, UnsavedFiles);
			results = new CodeCompletionResults {Completions = completions};

			return results;
		}

		public void ScrollToLine(int line)
		{
			Editor?.ScrollToLine(line);
		}

		public event EventHandler<EventArgs> DocumentLoaded;
		public event EventHandler<EventArgs> TextChanged;

		public void UnRegisterLanguageService()
		{
			ShutdownBackgroundWorkers();

            var unsavedFile = UnsavedFiles.BinarySearch(ProjectFile.Location);

			if (unsavedFile != null)
			{
				UnsavedFiles.Remove(unsavedFile);
			}

			if (LanguageService != null && ProjectFile != null)
			{
				LanguageService.UnregisterSourceFile(Editor, ProjectFile);
			}
		}

		public async void RegisterLanguageService(IIntellisenseControl intellisenseControl,
			ICompletionAssistant completionAssistant)
		{
			UnRegisterLanguageService();

            LanguageService = shell.LanguageServices.FirstOrDefault(o => o.CanHandle(ProjectFile));

            if (LanguageService != null)
            {
                ShellViewModel.Instance.StatusBar.Language = LanguageService.Title;

                LanguageService.RegisterSourceFile(intellisenseControl, completionAssistant, Editor, ProjectFile, TextDocument);
            }
            else
            {
                LanguageService = null;
                ShellViewModel.Instance.StatusBar.Language = "Text";
            }

			IsDirty = false;


			StartBackgroundWorkers();

			TextDocument.TextChanged += TextDocument_TextChanged;

			OnBeforeTextChanged(null);

			await TriggerCodeAnalysis();
		}

		public void OpenFile(ISourceFile file, IIntellisenseControl intellisense,
			ICompletionAssistant completionAssistant)
		{
			if (ProjectFile != file)
			{
				if (System.IO.File.Exists(file.Location))
				{
					using (var fs = System.IO.File.OpenText(file.Location))
					{
                        TextDocument = new TextDocument(fs.ReadToEnd());
                        TextDocument.FileName = file.Location;
					}

                    ProjectFile = file;

                    RegisterLanguageService(intellisense, completionAssistant);

                    DocumentLoaded?.Invoke(this, new EventArgs());
                }
			}
		}        

        public void Save()
		{
			if (ProjectFile != null && TextDocument != null && IsDirty)
			{
                System.IO.File.WriteAllText(ProjectFile.Location, TextDocument.Text);
				IsDirty = false;

                var unsavedFile = UnsavedFiles.BinarySearch(ProjectFile.Location);

				if (unsavedFile != null)
				{
					UnsavedFiles.Remove(unsavedFile);
				}
			}
		}

		private void TextDocument_TextChanged(object sender, EventArgs e)
		{
            var unsavedFile = UnsavedFiles.BinarySearch(ProjectFile.Location);

            if (unsavedFile == null)
			{
				UnsavedFiles.InsertSorted(new UnsavedFile(ProjectFile.Location, TextDocument.Text));
			}
			else
			{
				unsavedFile.Contents = TextDocument.Text;
			}

			IsDirty = true;

            TextChanged?.Invoke(this, new EventArgs());
        }

		public event EventHandler<EventArgs> CodeAnalysisCompleted;

		private void StartBackgroundWorkers()
		{
			cancellationSource = new CancellationTokenSource();

			Task.Factory.StartNew(() =>
			{
				codeAnalysisRunner.RunLoop(cancellationSource.Token);
				cancellationSource = null;
			});
		}

		public void ShutdownBackgroundWorkers()
		{
			cancellationSource?.Cancel();
		}

		public void OnBeforeTextChanged(object param)
		{
		}

		/// <summary>
		///     Write lock must be held before calling this.
		/// </summary>
		private async Task TriggerCodeAnalysis()
		{
			await codeAnalysisRunner.InvokeAsync(async () =>
			{
				if (LanguageService != null)
				{
					// TODO allow interruption.
					var result = await LanguageService.RunCodeAnalysisAsync(ProjectFile, UnsavedFiles, () => false);

					Dispatcher.UIThread.InvokeAsync(() => { CodeAnalysisResults = result; });
				}
			});
		}

		public async void OnTextChanged(object param)
		{
			IsDirty = true;

			await TriggerCodeAnalysis();
		}
	}
}