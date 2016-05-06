﻿namespace AvalonStudio.Debugging
{
    using AvalonStudio.Controls;
    using AvalonStudio.Documents;
    using AvalonStudio.Extensibility;
    using AvalonStudio.MVVM;
    using AvalonStudio.Platforms;
    using AvalonStudio.Projects;
    using AvalonStudio.Toolchains;
    using AvalonStudio.Utils;
    using Extensibility.Plugin;
    using Extensibility.Utils;
    using Perspex.Threading;
    using ReactiveUI;
    using Shell;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class DebugManager : ViewModel, IDebugManager, IExtension
    {
        private IShell _shell;
        private IConsole _console;

        #region Contructors
        public DebugManager()
        {
           BreakPointManager = new BreakPointManager();

            //Locals = new LocalsViewModel();
            //Registers = new RegistersViewModel();
            //Disassembly = new DisassemblyViewModel();
            //MemoryView = new MemoryViewModel();
            //WatchList = new WatchListViewModel();
            //VariableProbes = new List<VariableProbeViewModel>();
            //CallStack = new CallStackViewModel();
            
            StartDebuggingCommand = ReactiveCommand.Create();
            StartDebuggingCommand.Subscribe((o) =>
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

                            Task.Factory.StartNew((Func<Task>)(async () =>
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
                                        }//));
                                    }
                                    else
                                    {
                                        _console.WriteLine((string)"You have not selected a debug adaptor. Please goto Tools->Options to configure your debug adaptor.");
                                    }
                                }

                                //WorkspaceViewModel.Instance.ExecutingCompileTask = false;
                            }));
                        }
                        break;

                    case Perspective.Debug:
                        // Begin dispatch otherwise UI is awaiting debugger. Inversion of control.
                        //WorkspaceViewModel.Instance.BeginDispatchDebug(() =>
                        {
                            PrepareToRun();
                            CurrentDebugger.Continue();
                        }//);
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

            RestartDebuggingCommand = ReactiveCommand.Create(); //(o) => WorkspaceViewModel.Instance.CurrentPerspective == Perspective.Debug && Debugger != null && !IsUpdating);
            RestartDebuggingCommand.Subscribe(_ =>
            {
                SetDebuggers(currentDebugger);

                // Begin dispatch other wise ui thread is awaiting the debug thread. Inversion of control.
                //WorkspaceViewModel.Instance.BeginDispatchDebug(() =>
                {
                    PrepareToRun();
                    //Debugger.Reset(project.UserData.RunImmediately);
                }//);
            });

            StopDebuggingCommand = ReactiveCommand.Create(); //(o) => WorkspaceViewModel.Instance.CurrentPerspective == Perspective.Debug && Debugger != null && !IsUpdating);
            StopDebuggingCommand.Subscribe(_ =>
            {
                StopDebugSession();
            });

            InterruptDebuggingCommand = ReactiveCommand.Create(); //, (o) => WorkspaceViewModel.Instance.CurrentPerspective == Perspective.Debug && Debugger != null && Debugger.State == DebuggerState.Running && !IsUpdating);
            InterruptDebuggingCommand.Subscribe(_ =>
            {
                // Begin dispatch otherwise we would be on the ui thread awaiting the debugger.
                //WorkspaceViewModel.Instance.BeginDispatchDebug(() =>
                {
                    CurrentDebugger.Pause();
                }//);
            });

            StepIntoCommand = ReactiveCommand.Create();  //stepcommand can execute.
            StepIntoCommand.Subscribe(_ =>
            {
                StepInto();
            });

            StepInstructionCommand = ReactiveCommand.Create(); // }, StepCommandsCanExecute);
            StepInstructionCommand.Subscribe(_ =>
            {
                StepInstruction();
            });

            StepOverCommand = ReactiveCommand.Create(); // }, StepCommandsCanExecute);
            StepOverCommand.Subscribe(_ =>
            {
                StepOver();
            });

            StepOutCommand = ReactiveCommand.Create(); // , StepCommandsCanExecute);
            StepOutCommand.Subscribe(_ =>
            {
                StepOut();
            });
        }
        #endregion

        private bool IsExecuting = false;
        private bool IsUpdating = false;
        private IEditor lastDocument;

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

        public VariableObject ProbeExpression (string expression)
        {
            VariableObject result = null;

            if(CurrentDebugger.State == DebuggerState.Paused)
            {
                result = CurrentDebugger.CreateWatch(string.Format("probe{0}", CurrentDebugger.GetVariableId()), expression);
            }

            return result;
        }

        public void Continue ()
        {
            if (CurrentDebugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        CurrentDebugger.Continue();
                    });
                }
            }
        }

        public void StepInstruction()
        {
            if (CurrentDebugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        CurrentDebugger.StepInstruction();
                    });
                }
            }
        }

        public void StepOut()
        {
            if (CurrentDebugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        CurrentDebugger.StepOut();
                    });
                }
            }
        }

        public void StepInto()
        {
            if (CurrentDebugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        CurrentDebugger.StepInto();
                    });
                }
            }            
        }

        public void StepOver()
        {
            if (CurrentDebugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        CurrentDebugger.StepOver();
                    });
                }
            }
        }

        public void Pause()
        {
            if (CurrentDebugger != null)
            {
                if (IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        CurrentDebugger.Pause();
                    });
                }
            }
        }

        public void Stop()
        {
            if (CurrentDebugger != null)
            {                
                Task.Factory.StartNew(() =>
                {
                    PrepareToRun();

                    ignoreEvents = true;                    

                    StopDebugSession();

                    ignoreEvents = false;
                });
            }
        }

        public void Restart()
        {
            if (CurrentDebugger != null)
            {
                Task.Factory.StartNew(() =>
                {
                    CurrentDebugger.Reset(true);
                });             
            }
        }

        private bool ignoreEvents = false;

        private void StopDebugSession()
        {
            ignoreEvents = true;

            //WorkspaceViewModel.Instance.BeginDispatchDebug(() =>
            {
                CurrentDebugger.Stop();
            }//);

            //WatchList.Clear();
            //RegistersView.Clear();
            //LocalsView.Clear();
            //CallStack.Clear();

            SetDebuggers(null);

            ignoreEvents = false;

            Project = null;

            if (lastDocument != null)
            {
                lastDocument.ClearDebugHighlight();
                lastDocument = null;
            }

            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _shell.CurrentPerspective = Perspective.Editor;
            });
        }

        #region Commands
        public ReactiveCommand<object> StartDebuggingCommand { get; private set; }
        public ReactiveCommand<object> RestartDebuggingCommand { get; private set; }
        public ReactiveCommand<object> InterruptDebuggingCommand { get; private set; }
        public ReactiveCommand<object> StopDebuggingCommand { get; private set; }
        public ReactiveCommand<object> StepIntoCommand { get; private set; }
        public ReactiveCommand<object> StepOutCommand { get; private set; }
        public ReactiveCommand<object> StepOverCommand { get; private set; }
        public ReactiveCommand<object> StepInstructionCommand { get; private set; }
        #endregion

        private void SetDebuggers(IDebugger debugger)
        {
            BreakPointManager.SetDebugger(debugger);

            if(DebuggerChanged != null)
            {
                DebuggerChanged(this, new EventArgs());
            }
        }

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
                    currentDebugger.Close();
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


        private bool debugControlsVisible = false;
        public bool DebugControlsVisible
        {
            get { return debugControlsVisible; }
            set { this.RaiseAndSetIfChanged(ref debugControlsVisible, value); }
        }
        #endregion

        #region Private Fields
        //private EditorViewModel lastDocument;
        private bool closeLastDocument;
        #endregion

        #region Methods
        public void StartDebug(IProject project)
        {
            if (project?.Debugger != null)
            {
                Project = project;

                Task.Factory.StartNew(async () =>
                {
                    CurrentDebugger = project.Debugger;

                    await project.ToolChain.Build(_console, project);
                    //Debugger.DebugMode = true;

                    CurrentDebugger.Initialise();

                    _console.WriteLine();
                    _console.WriteLine("Starting Debugger...");

                    if (CurrentDebugger.Start(project.ToolChain, _console, project))
                    {
                        Dispatcher.UIThread.InvokeAsync(() =>
                        {
                            _shell.CurrentPerspective = Perspective.Debug;
                        });

                        BreakPointManager.GoLive();

                        await BreakPointManager.Add(CurrentDebugger.BreakMain());

                        CurrentDebugger.Run();
                    }
                });
            }
            else
            {
                _console.WriteLine("No debugger selected.");
            }
        }

        private IProject project;

        public event EventHandler<FrameChangedEventArgs> DebugFrameChanged;
        public event EventHandler DebuggerChanged;

        public IProject Project
        {
            get { return project; }
            set { project = value; }
        }

        // This is a hack to synchronise with the ui lists which update.
        private bool IsAsynchronousUIListsLoading()
        {
            bool result = false;

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
                    this.StopDebuggingCommand.Execute(null);

                    Dispatcher.UIThread.InvokeAsync( () =>
                    {
                        _shell.CurrentPerspective = Perspective.Editor;
                    });
                    break;

                default:
                    IsUpdating = true;

                    if (e.Frame != null && e.Frame.File != null)
                    {
                        var normalizedPath = e.Frame.File.Replace("\\\\","\\").ToPlatformPath();

                        ISourceFile file = null;

                        var document = _shell.GetDocument(normalizedPath);
                        file = document?.ProjectFile;

                        if (file == null)
                        {
                            file = _shell.CurrentSolution.FindFile(PathSourceFile.FromPath(null, null, normalizedPath));
                        }

                        if (file != null)
                        {
                            await Dispatcher.UIThread.InvokeTaskAsync(async () =>
                            {
                                document = await _shell.OpenDocument(file, e.Frame.Line, 1, true);
                            });
                        }
                        else
                        {
                            _console.WriteLine("Unable to find file: " + normalizedPath);
                        }

                        lastDocument = document;
                    }
                    
                    if(DebugFrameChanged != null)
                    {
                        FrameChangedEventArgs args = new FrameChangedEventArgs();
                        args.Address = e.Frame.Address;
                        args.VariableChanges = currentDebugger.UpdateVariables();
                        

                        DebugFrameChanged(this, args);
                    }

                    IsUpdating = false;
                    IsExecuting = false;
                    break;
            }
        }

        void debugger_StateChanged(object sender, EventArgs e)
        {
            // WorkspaceViewModel.Instance.BeginDispatchUi(() =>
            {
                //CommandManager.InvalidateRequerySuggested();
            }//);
        }

        bool StepCommandsCanExecute(object arguments)
        {
            bool result = false;

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
            IoC.RegisterConstant(this, typeof(IDebugManager));
        }

        public void Activation()
        {
            _shell = IoC.Get<IShell>();
            _console = IoC.Get<IConsole>();
        }
        #endregion
    }
}
