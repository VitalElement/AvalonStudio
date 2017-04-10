using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using AvalonStudio.Documents;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.Extensibility.Utils;
using AvalonStudio.MVVM;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using ReactiveUI;

namespace AvalonStudio.Debugging
{
	public class DebugManager : ViewModel, IDebugManager, IExtension
	{
		private IConsole _console;
		private IShell _shell;
		private bool ignoreEvents;
		private bool IsExecuting;
		private bool IsUpdating;
		private IEditor lastDocument;

		#region Contructors

		public DebugManager()
		{
			BreakPointManager = new BreakPointManager();

			StartDebuggingCommand = ReactiveCommand.Create();
			StartDebuggingCommand.Subscribe(async o =>
			{
				switch (_shell.CurrentPerspective)
				{
					case Perspective.Editor:
						if (o == null)
						{
							o = _shell.CurrentSolution.StartupProject;
						}

						if (o is IProject)
						{
							var masterProject = o as IProject;

							//WorkspaceViewModel.Instance.SaveAllCommand.Execute(null);

							Task.Factory.StartNew(async () =>
							{
								//WorkspaceViewModel.Instance.ExecutingCompileTask = true;

								if (await masterProject.ToolChain.Build(_console, masterProject))
								{
									//Debugger = masterProject.SelectedDebugAdaptor;

									if (CurrentDebugger != null)
									{
										//await WorkspaceViewModel.Instance.DispatchDebug((Action)(() =>
										{
											StartDebug(masterProject);
										} //));
									}
									else
									{
										_console.WriteLine(
											"You have not selected a debug adaptor. Please goto Tools->Options to configure your debug adaptor.");
									}
								}

								//WorkspaceViewModel.Instance.ExecutingCompileTask = false;
							});
						}
						break;

					case Perspective.Debug:
						// Begin dispatch otherwise UI is awaiting debugger. Inversion of control.
						//WorkspaceViewModel.Instance.BeginDispatchDebug(() =>
					{
						PrepareToRun();
						await CurrentDebugger.ContinueAsync();
					} //);
						break;
				}
			} /*, (o) =>
            {
                bool result = false;

                switch (WorkspaceViewModel.Instance.CurrentPerspective)
                {
                    case Perspective.Editor:
                        if (WorkspaceViewModel.Instance.SolutionExplorer.SelectedProject != null && !WorkspaceViewModel.Instance.ExecutingCompileTask)
                        {
                            result = true;
                        }
                        break;

                    case Perspective.Debug:
                        result = StepCommandsCanExecute(o);
                        break;
                }

                return result;
            }*/);

			RestartDebuggingCommand = ReactiveCommand.Create();
				//(o) => WorkspaceViewModel.Instance.CurrentPerspective == Perspective.Debug && Debugger != null && !IsUpdating);
			RestartDebuggingCommand.Subscribe(_ =>
			{
				SetDebuggers(currentDebugger);

				// Begin dispatch other wise ui thread is awaiting the debug thread. Inversion of control.
				//WorkspaceViewModel.Instance.BeginDispatchDebug(() =>
				{
					PrepareToRun();
					//Debugger.Reset(project.UserData.RunImmediately);
				} //);
			});

			StopDebuggingCommand = ReactiveCommand.Create();
				//(o) => WorkspaceViewModel.Instance.CurrentPerspective == Perspective.Debug && Debugger != null && !IsUpdating);
			StopDebuggingCommand.Subscribe(_ => { Stop(); });

			InterruptDebuggingCommand = ReactiveCommand.Create();
				//, (o) => WorkspaceViewModel.Instance.CurrentPerspective == Perspective.Debug && Debugger != null && Debugger.State == DebuggerState.Running && !IsUpdating);
			InterruptDebuggingCommand.Subscribe(async _ => { await CurrentDebugger.PauseAsync(); });

			StepIntoCommand = ReactiveCommand.Create(); //stepcommand can execute.
			StepIntoCommand.Subscribe(_ => { StepInto(); });

			StepInstructionCommand = ReactiveCommand.Create(); // }, StepCommandsCanExecute);
			StepInstructionCommand.Subscribe(_ => { StepInstruction(); });

			StepOverCommand = ReactiveCommand.Create(); // }, StepCommandsCanExecute);
			StepOverCommand.Subscribe(_ => { StepOver(); });

			StepOutCommand = ReactiveCommand.Create(); // , StepCommandsCanExecute);
			StepOutCommand.Subscribe(_ => { StepOut(); });
		}

		#endregion

		public async Task<VariableObject> ProbeExpressionAsync(string expression)
		{
			VariableObject result = null;

			/*if (CurrentDebugger.State == DebuggerState.Paused)
			{
				result =
					await CurrentDebugger.CreateWatchAsync(string.Format("probe{0}", CurrentDebugger.GetVariableId()), expression);
			}*/

			return result;
		}

		public async void Continue()
		{
			if (CurrentDebugger != null)
			{
				if (!IsExecuting)
				{
					PrepareToRun();
					await CurrentDebugger.ContinueAsync();
				}
			}
		}

		public async void StepInstruction()
		{
			if (CurrentDebugger != null)
			{
				if (!IsExecuting)
				{
					PrepareToRun();
					await CurrentDebugger.StepInstructionAsync();
				}
			}
		}

		public async void StepOut()
		{
			if (CurrentDebugger != null)
			{
				if (!IsExecuting)
				{
					PrepareToRun();
					await CurrentDebugger.StepOutAsync();
				}
			}
		}

		public async void StepInto()
		{
			if (CurrentDebugger != null)
			{
				if (!IsExecuting)
				{
					PrepareToRun();
					await CurrentDebugger.StepIntoAsync();
				}
			}
		}

		public async void StepOver()
		{
			if (CurrentDebugger != null)
			{
				if (!IsExecuting)
				{
					PrepareToRun();
					await CurrentDebugger.StepOverAsync();
				}
			}
		}

		public async void Pause()
		{
			if (CurrentDebugger != null)
			{
				if (IsExecuting)
				{
					await CurrentDebugger.PauseAsync();
				}
			}
		}

		public void Stop()
		{
			if (CurrentDebugger != null)
			{
                if (!IsUpdating)
                {
                    PrepareToRun();

                    ignoreEvents = true;

                    StopDebugSession();

                    ignoreEvents = false;
                }
			}
		}

		public async void Restart()
		{
			if (CurrentDebugger != null)
			{
                PrepareToRun();
				await CurrentDebugger.ResetAsync(true);
			}
		}

		private void PrepareToRun()
		{
			if (lastDocument != null)
			{
				lastDocument.ClearDebugHighlight();
			}

			//foreach (var probe in VariableProbes)
			//{
			//    probe.Close();
			//}

			IsExecuting = true;
		}

		private void ResetDebugSession()
		{
			CurrentDebugger = null;
			SetDebuggers(null);

			Project = null;

			if (lastDocument != null)
			{
				lastDocument.ClearDebugHighlight();
				lastDocument = null;
			}

			Dispatcher.UIThread.InvokeAsync(() =>
			{
				if (DebugSessionEnded != null)
				{
					DebugSessionEnded(this, new EventArgs());
				}

				_shell.CurrentPerspective = Perspective.Editor;
			});
		}

		private async void StopDebugSession()
		{
			ignoreEvents = true;

			await CurrentDebugger.StopAsync();

			ignoreEvents = false;

			ResetDebugSession();
		}

		private void SetDebuggers(IDebugger debugger)
		{
			BreakPointManager.SetDebugger(debugger);

			if (DebuggerChanged != null)
			{
				DebuggerChanged(this, new EventArgs());
			}
		}

		#region Commands

		public ReactiveCommand<object> StartDebuggingCommand { get; }
		public ReactiveCommand<object> RestartDebuggingCommand { get; }
		public ReactiveCommand<object> InterruptDebuggingCommand { get; }
		public ReactiveCommand<object> StopDebuggingCommand { get; }
		public ReactiveCommand<object> StepIntoCommand { get; }
		public ReactiveCommand<object> StepOutCommand { get; }
		public ReactiveCommand<object> StepOverCommand { get; }
		public ReactiveCommand<object> StepInstructionCommand { get; }

		#endregion

		#region Properties

		private IDebugger currentDebugger;

		public IDebugger CurrentDebugger
		{
			get { return currentDebugger; }
			set
			{
				if (currentDebugger != null)
				{
					currentDebugger.Stopped -= debugger_Stopped;
					currentDebugger.StateChanged -= debugger_StateChanged;
					currentDebugger.CloseAsync();
				}

				currentDebugger = value;

				if (value != null)
				{
					value.Stopped += debugger_Stopped;
					value.StateChanged += debugger_StateChanged;

					SetDebuggers(value);
				}

				this.RaisePropertyChanged(nameof(CurrentDebugger));
			}
		}

		//public List<VariableProbeViewModel> VariableProbes { get; private set; }
		public BreakPointManager BreakPointManager { get; set; }

		//private LocalsViewModel locals;
		//public LocalsViewModel Locals
		//{
		//    get { return locals; }
		//    set { this.RaiseAndSetIfChanged(ref locals, value); }
		//}

		//private WatchListViewModel watchList;
		//public WatchListViewModel WatchList
		//{
		//    get { return watchList; }
		//    set { this.RaiseAndSetIfChanged(ref watchList, value); }
		//}

		//private CallStackViewModel callStack;
		//public CallStackViewModel CallStack
		//{
		//    get { return callStack; }
		//    set { this.RaiseAndSetIfChanged(ref callStack, value); }
		//}

		//private RegistersViewModel registersView;
		//public RegistersViewModel Registers
		//{
		//    get { return registersView; }
		//    set { this.RaiseAndSetIfChanged(ref registersView, value); }
		//}

		//private DisassemblyViewModel disassembly;
		//public DisassemblyViewModel Disassembly
		//{
		//    get { return disassembly; }
		//    set { this.RaiseAndSetIfChanged(ref disassembly, value); }
		//}

		//private MemoryViewModel memoryView;
		//public MemoryViewModel MemoryView
		//{
		//    get { return memoryView; }
		//    set { memoryView = value; OnPropertyChanged(); }
		//}


		private bool debugControlsVisible;

		public bool DebugControlsVisible
		{
			get { return debugControlsVisible; }
			set { this.RaiseAndSetIfChanged(ref debugControlsVisible, value); }
		}

		#endregion

		#region Methods

		public void StartDebug(IProject project)
		{
			if (project?.Debugger != null)
			{
				Project = project;

				Task.Factory.StartNew(async () =>
				{
					await project.ToolChain.Build(_console, project);
					//Debugger.DebugMode = true;

					project.Debugger.Initialise();

					_console.WriteLine();
					_console.WriteLine("Starting Debugger...");

					if (await project.Debugger.StartAsync(project.ToolChain, _console, project))
					{
						Dispatcher.UIThread.InvokeAsync(() =>
						{
							if (DebugSessionStarted != null)
							{
								DebugSessionStarted(this, new EventArgs());
							}

							_shell.CurrentPerspective = Perspective.Debug;
						});

						CurrentDebugger = project.Debugger;

						await BreakPointManager.GoLiveAsync();

						await BreakPointManager.Add(await CurrentDebugger.BreakMainAsync());

						await CurrentDebugger.RunAsync();
					}
					else
					{
						_console.WriteLine("Unable to connect to debugger.");
						ResetDebugSession();
					}
				});
			}
			else
			{
				_console.WriteLine("No debugger selected.");
			}
		}

		public event EventHandler<FrameChangedEventArgs> DebugFrameChanged;
		public event EventHandler DebuggerChanged;
		public event EventHandler DebugSessionStarted;
		public event EventHandler DebugSessionEnded;

		public IProject Project { get; set; }

		// This is a hack to synchronise with the ui lists which update.
		private bool IsAsynchronousUIListsLoading()
		{
			var result = false;

			/*if (DissasemblyView.DissasemblyData != null && MemoryView.MemoryData != null)
            {
                WorkspaceViewModel.Instance.DispatchUi(() =>
                {
                    result = (DissasemblyView.DissasemblyData.RequestsWaiting != 0 || DissasemblyView.DissasemblyData.IsLoading) || (MemoryView.MemoryData.RequestsWaiting != 0 || MemoryView.MemoryData.IsLoading);
                });
            }*/

			return result;
		}

		private async void debugger_Stopped(object sender, StopRecord e)
		{
			if (ignoreEvents)
			{
				return;
			}

			switch (e.Reason)
			{
				case StopReason.ExitedNormally:
				case StopReason.Exited:
				case StopReason.ExitedSignalled:
					IsExecuting = false;
					IsUpdating = false;
					StopDebugSession();
					break;

				default:
					IsUpdating = true;

					if (DebugFrameChanged != null)
					{
						var args = new FrameChangedEventArgs();
						args.Address = e.Frame.Address;
						args.VariableChanges = await currentDebugger.UpdateVariablesAsync();

						DebugFrameChanged(this, args);
					}

                    if (e.Frame != null && e.Frame.FullFileName != null)
                    {
                        var normalizedPath = e.Frame.FullFileName.Replace("\\\\", "\\").NormalizePath();

                        ISourceFile file = null;

                        var document = _shell.GetDocument(normalizedPath);

                        if (document != null)
                        {
                            lastDocument = document;
                            file = document?.ProjectFile;
                        }
                        
                        if (file == null)
                        {
                            file = _shell.CurrentSolution.FindFile(normalizedPath);
                        }

                        if (file != null)
                        {
                            await
                                Dispatcher.UIThread.InvokeTaskAsync(
                                    async () => { lastDocument = document = await _shell.OpenDocument(file, e.Frame.Line, -1, -1, true); });
                        }
                        else
                        {
                            _console.WriteLine("Unable to find file: " + normalizedPath);
                        }
                    }

                    IsUpdating = false;
                    IsExecuting = false;
                    break;
			}
		}

		private void debugger_StateChanged(object sender, EventArgs e)
		{
			// WorkspaceViewModel.Instance.BeginDispatchUi(() =>
			{
				//CommandManager.InvalidateRequerySuggested();
			} //);
		}

		private bool StepCommandsCanExecute(object arguments)
		{
			var result = false;

			if (_shell.CurrentPerspective == Perspective.Debug && CurrentDebugger != null)
			{
				if (CurrentDebugger.State == DebuggerState.Paused && !IsExecuting)
				{
					result = true;
				}
			}

			return result;
		}

		public void BeforeActivation()
		{
			IoC.RegisterConstant(this, typeof (IDebugManager));
		}

		public void Activation()
		{
			_shell = IoC.Get<IShell>();
			_console = IoC.Get<IConsole>();
		}

		#endregion
	}
}