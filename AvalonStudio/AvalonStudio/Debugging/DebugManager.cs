﻿using AvalonStudio.Controls;
using AvalonStudio.Debugging.GDB.OpenOCD;
using AvalonStudio.Extensibility;
using AvalonStudio.Models.Tools.Debuggers.Local;
using AvalonStudio.MVVM;
using AvalonStudio.Projects;
using AvalonStudio.Projects.Standard;
using AvalonStudio.Toolchains;
using AvalonStudio.Utils;
using Perspex.Threading;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging
{
    public class DebugManager : ViewModel
    {
        #region Contructors
        public DebugManager()
        {
            this.BreakPointManager = new BreakPointManager();

            Locals = new LocalsViewModel();
            Registers = new RegistersViewModel();
            Disassembly = new DisassemblyViewModel();
            //MemoryView = new MemoryViewModel();
            //WatchList = new WatchListViewModel();
            //VariableProbes = new List<VariableProbeViewModel>();
            CallStack = new CallStackViewModel();

            //LocalsView.bool = bool.Hidden;
            //RegistersView.bool = bool.Hidden;
            //DissasemblyView.bool = bool.Hidden;
            //MemoryView.bool = bool.Hidden;
            //WatchList.bool = bool.Hidden;
            //CallStack.bool = bool.Hidden;

            StartDebuggingCommand = ReactiveCommand.Create();
            StartDebuggingCommand.Subscribe((o) =>
            {
                switch (WorkspaceViewModel.Instance.CurrentPerspective)
                {
                    case Perspective.Editor:
                        if (o == null)
                        {
                            o = WorkspaceViewModel.Instance.SolutionExplorer.Model.StartupProject;
                        }

                        if (o is IProject)
                        {
                            var masterProject = o as IProject;

                            //WorkspaceViewModel.Instance.SaveAllCommand.Execute(null);

                            Task.Factory.StartNew((Func<Task>)(async () =>
                            {
                                //WorkspaceViewModel.Instance.ExecutingCompileTask = true;

                                if (await masterProject.ToolChain.Build(WorkspaceViewModel.Instance.Console, masterProject))
                                {
                                    //Debugger = masterProject.SelectedDebugAdaptor;

                                    if (Debugger != null)
                                    {
                                        //await WorkspaceViewModel.Instance.DispatchDebug((Action)(() =>
                                        {
                                            StartDebug(masterProject);
                                        }//));
                                    }
                                    else
                                    {
                                        WorkspaceViewModel.Instance.Console.WriteLine((string)"You have not selected a debug adaptor. Please goto Tools->Options to configure your debug adaptor.");
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
                            Debugger.Continue();
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
                SetDebuggers(debugger);

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
                    Debugger.Pause();
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

        private void PrepareToRun()
        {
            WorkspaceViewModel.Instance.Editor.DebugLineHighlighter.Line = -1;
            //if (lastDocument != null)
            //{
            //    lastDocument.ClearDebugHighlight();
            //}

            //foreach (var probe in VariableProbes)
            //{
            //    probe.Close();
            //}

            IsExecuting = true;
        }

        public void Continue ()
        {
            if (Debugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        Debugger.Continue();
                    });
                }
            }
        }

        public void StepInstruction()
        {
            if (Debugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        Debugger.StepInstruction();
                    });
                }
            }
        }

        public void StepOut()
        {
            if (Debugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        Debugger.StepOut();
                    });
                }
            }
        }

        public void StepInto()
        {
            if (Debugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        Debugger.StepInto();
                    });
                }
            }            
        }

        public void StepOver()
        {
            if (Debugger != null)
            {
                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        Debugger.StepOver();
                    });
                }
            }
        }

        public void Pause()
        {
            if (Debugger != null)
            {
                if (IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        Debugger.Pause();
                    });
                }
            }
        }

        public void Stop()
        {
            if (Debugger != null)
            {                
                Task.Factory.StartNew(() =>
                {
                    if (IsExecuting)
                    {
                        Debugger.Pause();                     
                    }

                    PrepareToRun();

                    StopDebugSession();
                });
            }
        }

        public void Restart()
        {
            if (Debugger != null)
            {
                Pause();

                if (!IsExecuting)
                {
                    Task.Factory.StartNew(() =>
                    {
                        PrepareToRun();
                        Debugger.Reset(true);
                    });
                }
            }
        }

        private bool ignoreEvents = false;

        private void StopDebugSession()
        {
            ignoreEvents = true;

            //WorkspaceViewModel.Instance.BeginDispatchDebug(() =>
            {
                Debugger.Stop();
            }//);

            //WorkspaceViewModel.Instance.DispatchUi(() =>
            {
                //WatchList.Clear();
                //RegistersView.Clear();
                //LocalsView.Clear();
                //CallStack.Clear();

                SetDebuggers(null);

                ignoreEvents = false;

                Project = null;

                //if (lastDocument != null)
                //{
                //    lastDocument.ClearDebugHighlight();
                //}
            }//);

            WorkspaceViewModel.Instance.CurrentPerspective = Perspective.Editor;
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
            Registers.SetDebugger(debugger);
            Disassembly.SetDebugger(debugger);
            //MemoryView.SetDebugger(debugger);
            //WatchList.SetDebugger(debugger as GDBDebugger);
        }

        #region Properties
        private IDebugger debugger;
        public IDebugger Debugger
        {
            get { return debugger; }
            set
            {
                if (debugger != null)
                {
                    debugger.Stopped -= debugger_Stopped;
                    debugger.StateChanged -= debugger_StateChanged;
                    debugger.Close();
                }

                debugger = value;

                if (value != null)
                {
                    value.Stopped += debugger_Stopped;
                    value.StateChanged += debugger_StateChanged;

                    SetDebuggers(value);
                }

                this.RaisePropertyChanged(nameof(Debugger));
            }
        }

        //public List<VariableProbeViewModel> VariableProbes { get; private set; }
        public BreakPointManager BreakPointManager { get; set; }

        private LocalsViewModel locals;
        public LocalsViewModel Locals
        {
            get { return locals; }
            set { this.RaiseAndSetIfChanged(ref locals, value); }
        }

        //private WatchListViewModel watchList;
        //public WatchListViewModel WatchList
        //{
        //    get { return watchList; }
        //    set { watchList = value; OnPropertyChanged(); }
        //}

        private CallStackViewModel callStack;
        public CallStackViewModel CallStack
        {
            get { return callStack; }
            set { this.RaiseAndSetIfChanged(ref callStack, value); }
        }

        private RegistersViewModel registersView;
        public RegistersViewModel Registers
        {
            get { return registersView; }
            set { this.RaiseAndSetIfChanged(ref registersView, value); }
        }

        private DisassemblyViewModel disassembly;
        public DisassemblyViewModel Disassembly
        {
            get { return disassembly; }
            set { this.RaiseAndSetIfChanged(ref disassembly, value); }
        }

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
                    Debugger = project.Debugger;

                    await project.ToolChain.Build(WorkspaceViewModel.Instance.Console, project);
                    //Debugger.DebugMode = true;

                    Debugger.Initialise();

                    WorkspaceViewModel.Instance.Console.WriteLine();
                    WorkspaceViewModel.Instance.Console.WriteLine("Starting Debugger...");

                    if (Debugger.Start(project.ToolChain, WorkspaceViewModel.Instance.Console, project))
                    {
                        WorkspaceViewModel.Instance.CurrentPerspective = Perspective.Debug;

                        BreakPointManager.GoLive();

                        await BreakPointManager.Add(Debugger.BreakMain());

                        Debugger.Run();
                    }
                });
            }
            else
            {
                WorkspaceViewModel.Instance.Console.WriteLine("No debugger selected.");
            }
        }

        private IProject project;
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
                    WorkspaceViewModel.Instance.CurrentPerspective = Perspective.Editor;
                    break;

                default:
                    IsUpdating = true;

                    if (e.Frame != null && e.Frame.File != null)
                    {                        
                        var normalizedPath = e.Frame.File.NormalizePath();

                        var file = WorkspaceViewModel.Instance.Editor.Model.ProjectFile;

                        if (file == null || file.File != normalizedPath)
                        {
                            file = WorkspaceViewModel.Instance.SolutionExplorer.Model.FindFile(normalizedPath);
                        }

                        if (file != null)
                        {
                            Dispatcher.UIThread.InvokeAsync(() =>
                            {
                                WorkspaceViewModel.Instance.Editor.OpenFile(file, e.Frame.Line, 1, true);
                            });
                        }
                        else
                        {
                            WorkspaceViewModel.Instance.Console.WriteLine("Unable to find file: " + e.Frame.File);
                        }
                    }


                    List<Variable> stackVariables = null;
                    List<Frame> stackFrames = null;

                    stackVariables = Debugger.ListStackVariables();
                    stackFrames = Debugger.ListStackFrames();
                    //updates = debugger.UpdateVariables();

                    //if (DissasemblyView.bool == bool.Visible)
                    //{
                    //   WorkspaceViewModel.Instance.DispatchUi(() =>
                    //    {                    
                    //  });
                    // }

                    //if (MemoryView.bool == bool.Visible)
                    //{
                    //    MemoryView.Invalidate();
                    //}
                    

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        Locals.Model = stackVariables;
                        CallStack.Update(stackFrames);
                        Registers.Invalidate();
                        Disassembly.SetAddress(e.Frame.Address);
                    });

                    //WatchList.Invalidate(updates);
                    

                    //while (await IsAsynchronousUIListsLoading())
                    //{
                    //    Thread.Sleep(1);
                    //}

                    //WorkspaceViewModel.Instance.BeginDispatchUi(() =>
                    {
                        //CommandManager.InvalidateRequerySuggested();
                    }//);

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

            if (WorkspaceViewModel.Instance.CurrentPerspective == Perspective.Debug && Debugger != null)
            {
                if (Debugger.State == DebuggerState.Paused && !IsExecuting)
                {
                    result = true;
                }
            }

            return result;
        }
        #endregion
    }
}
