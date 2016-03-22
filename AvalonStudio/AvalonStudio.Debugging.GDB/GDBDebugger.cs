﻿namespace AvalonStudio.Debugging.GDB
{
    using AvalonStudio.Utils;
    using Perspex.Controls;
    using Platform;
    using Projects;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Xml.Serialization;
    using Toolchains;
    using Toolchains.GCC;

    public class GDBDebugger : IDebugger
    {
        public GDBDebugger()
        {
            stoppedEventIsEnabled = true;
        }

        ~GDBDebugger()
        {
            if (process != null && !process.HasExited)
            {
                process.Kill();
            }
        }

        protected IConsole console;

        private DebuggerState currentState = DebuggerState.NotRunning;

        protected DebuggerState CurrentState
        {
            get { return currentState; }
            set
            {
                if (currentState != value)
                {
                    currentState = value;

                    if (StateChanged != null)
                    {
                        StateChanged(this, new EventArgs());
                    }
                }
            }
        }

        public bool DebugMode { get; set; }

        private Command currentCommand;

        internal Command SetCommand(Command command)
        {
            if (currentCommand != null)
            {
                console.WriteLine("Serious error.");
            }

            this.currentCommand = command;
            return command;
        }
        internal void ClearCommand()
        {
            this.currentCommand = null;
        }

        private string WaitForResponse(Int32 timeout)
        {
            string result = string.Empty;

            if (responseReceived.WaitOne(timeout))
            {
                if (response == "^running")
                {
                    CurrentState = DebuggerState.Running;
                }

                result = response;
                response = string.Empty;
            }
            else if (DebugMode)
            {
                console.WriteLine("gdb command timed out.");
            }

            return result;
        }

        public List<MemoryBytes> ReadMemoryBytes(ulong address, ulong offset, uint count)
        {
            List<MemoryBytes> result = null;

            SafelyExecuteCommand(() =>
            {
                result = new DataReadMemoryBytesCommand(address, offset, count).Execute(this).Value;
            });

            return result;
        }

        public List<VariableObjectChange> UpdateVariables()
        {
            GDBResponse<List<VariableObjectChange>> response = null;

            response = new VarUpdateCommand().Execute(this);

            if (response.Response == ResponseCode.Done)
            {
                return response.Value;
            }
            else
            {
                return null;
            }
        }

        internal string EvaluateExpression(string expression)
        {
            if (CurrentState != DebuggerState.Paused)
            {
                throw new Exception("Expressions can only be evaluated in the paused state.");
            }

            var result = string.Empty;

            var response = new VarEvaluateExpressionCommand(expression).Execute(this);

            if (response.Response == ResponseCode.Done)
            {
                result = response.Value;
            }
            else
            {
                //throw new Exception ("expression not valid.");
            }

            return result;
        }

        internal List<VariableObject> ListChildren(VariableObject variable)
        {
            if (CurrentState != DebuggerState.Paused)
            {
                throw new Exception("Expressions can only be evaluated in the paused state.");
            }

            var response = new VarListChildrenCommand(variable).Execute(this);

            if (response.Response == ResponseCode.Done)
            {
                return response.Value;
            }
            else
            {
                return null;
            }
        }

        internal string SendCommand(Command command, Int32 timeout)
        {
            string result = string.Empty;
            transmitSemaphore.WaitOne();
            SetCommand(command);

            if (DebugMode)
            {
                console.WriteLine("[Sending] " + command.Encode());
            }

            input.WriteLine(command.Encode());
            input.Flush();

            result = WaitForResponse(-1);

            ClearCommand();

            transmitSemaphore.Release();

            return result;
        }



        public void Initialise()
        {
            responseReceived = new Semaphore(0, 1);
        }

        private StreamWriter input;
        private Semaphore responseReceived;
        private string response = string.Empty;
        private Process process;

        private void ProcessOutput(string data)
        {
            switch (data[0])
            {
                case '*':   // out of band data.
                    if (currentCommand != null)
                    {
                        currentCommand.OutOfBandDataReceived(data);
                    }

                    if (DebugMode)
                    {
                        console.WriteLine("[Async Record] " + data);
                    }

                    ProcessAsynRecord(data);
                    break;

                case '=':   // notification record
                    if (DebugMode)
                    {
                        console.WriteLine("[Notification] " + data);
                    }
                    break;

                case '^':   //this is for a response.
                    if (DebugMode)
                    {
                        console.WriteLine("[Response] " + data + Environment.NewLine);
                    }

                    if (currentCommand != null)
                    {
                        response += data;
                        responseReceived.Release();
                    }
                    else if (DebugMode)
                    {
                        console.WriteLine("Current Command NULL");
                    }
                    break;

                case '&':
                    if (data == "&\"Quit (expect signal SIGINT when the program is resumed)\\n\"")
                    {
                        if (InternalStopped != null)
                        {
                            InternalStopped(this, null);
                        }
                    }
                    break;
            }
        }

        private void ProcessAsynRecord(string data)
        {
            var split = data.Split(new char[] { ',' }, 2);

            switch (split[0].Replace("*", ""))
            {
                case "running":
                    CurrentState = DebuggerState.Running;
                    break;

                case "stopped":
                    ProcessStopRecord(split[1]);
                    break;
            }
        }

        private bool stoppedEventIsEnabled;

        [XmlIgnore]
        public bool StoppedEventIsEnabled
        {
            get { return stoppedEventIsEnabled; }
            set { stoppedEventIsEnabled = value; }
        }

        private void ProcessStopRecord(string data)
        {
            CurrentState = DebuggerState.Paused;

            var stopRecord = StopRecord.FromArgumentList(data.ToNameValuePairs());

            if (InternalStopped != null)
            {
                InternalStopped(this, stopRecord);
            }

            if (Stopped != null && StoppedEventIsEnabled)
            {
                Task.Factory.StartNew(() =>
                {
                    Stopped(this, stopRecord);
                });
            }
        }

        public List<Variable> ListStackVariables()
        {
            GDBResponse<List<Variable>> result = null;

            SafelyExecuteCommand(() =>
            {
                result = new StackListVariablesCommand().Execute(this);
            });

            if (result != null)
            {
                return result.Value;
            }
            else
            {
                return null;
            }
        }

        public List<Frame> ListStackFrames()
        {
            GDBResponse<List<Frame>> result = null;

            SafelyExecuteCommand(() =>
            {
                result = new StackListFramesCommand().Execute(this);
            });

            if (result.Response == ResponseCode.Done)
            {
                return result.Value;
            }
            else
            {
                return null;
            }
        }

        public LiveBreakPoint BreakMain()
        {
            return new SetBreakPointCommand("main").Execute(this).Value;
        }

        /// <summary>
        /// This method is not supported by embedded targets. Use continue instead.
        /// </summary>
        public virtual void Run()
        {
            if (CurrentState != DebuggerState.Running)
            {
                new RunCommand().Execute(this);
            }
        }

        private Semaphore waitForStop = new Semaphore(0, 1);
        private Semaphore transmitSemaphore = new Semaphore(1, 1);

        public bool Pause()
        {
            bool result = true;

            if (currentState == DebuggerState.Paused)
            {
                result = false;
                return result;
            }

            transmitSemaphore.WaitOne();

            EventHandler<StopRecord> onStoppedHandler = (sender, e) =>
            {
                if (e != null)
                {
                    switch (e.Reason)
                    {
                        case StopReason.SignalReceived:
                            break;

                        default:
                                // indicate that the debugger has been interrupted for a reason other than signalling, i.e. stepping range ended.
                                result = false;
                            break;
                    }
                }

                waitForStop.Release();
            };

            this.InternalStopped += onStoppedHandler;

            Platform.SendSignal(process.Id, Platform.Signum.SIGINT);

            waitForStop.WaitOne();

            this.InternalStopped -= onStoppedHandler;

            transmitSemaphore.Release();

            return result;
        }

        public void Continue()
        {
            if (CurrentState != DebuggerState.Running)
            {
                new ContinueCommand().Execute(this);
            }
        }

        public void StepOver()
        {
            new StepOverCommand().Execute(this);
        }

        public void StepOut()
        {
            new FinishCommand().Execute(this);
        }

        public void StepInstruction()
        {
            new ExecNextInstructionCommand().Execute(this);
        }

        public void StepInto()
        {
            new StepIntoCommand().Execute(this);
        }

        public virtual void Stop()
        {
            transmitSemaphore.WaitOne();
            input.WriteLine("-gdb-exit");
            transmitSemaphore.Release();
        }

        public void Close()
        {
            try
            {
                if (process != null && !process.HasExited)
                {
                    input.WriteLine("-gdb-exit");
                    process.WaitForExit();
                }
            }
            catch (Exception e)
            {
                // Work around for process becoming null between.
            }
        }

        public virtual bool Start(IToolChain toolchain, IConsole console, IProject project)
        {
            this.console = console;
            var startInfo = new ProcessStartInfo();

            console.WriteLine("[GDB] - Starting...");

            // This information should be part of this extension... or configurable internally?
            // This maybe indicates that debuggers are part of toolchain?

            if (toolchain is GCCToolchain)
            {
                startInfo.FileName = (toolchain as GCCToolchain).GDBExecutable;
            }
            else
            {
                console.WriteLine("[GDB] - Error GDB is not able to debug projects compiled on this kind of toolchain (" + toolchain.GetType().ToString() + ")");
                return false;
            }

            startInfo.Arguments = string.Format("--interpreter=mi \"{0}\"", Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath());

            if (!File.Exists(startInfo.FileName))
            {
                console.WriteLine("[GDB] - Error unable to find executable.");
                return false;
            }

            // Hide console window
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardInput = true;
            startInfo.CreateNoWindow = true;

            process = Process.Start(startInfo);

            input = process.StandardInput;

            while (!Platform.FreeConsole())
            {
                Console.WriteLine(Marshal.GetLastWin32Error());
                Thread.Sleep(10);
            }

            while (!Platform.AttachConsole(process.Id))
            {
                Thread.Sleep(10);
            }

            while (!Platform.SetConsoleCtrlHandler(null, true))
            {
                Console.WriteLine(Marshal.GetLastWin32Error());
                Thread.Sleep(10);
            }

            Task.Factory.StartNew(() =>
           {
               console.WriteLine("[GDB] - Started");


               process.OutputDataReceived += (sender, e) =>
               {
                   if (e.Data != null)
                   {
                       ProcessOutput(e.Data);
                   }
               };

               process.ErrorDataReceived += (sender, e) =>
               {
                   if (e.Data != null)
                   {
                       console.WriteLine("[GDB] - " + e.Data);
                   }
               };

               process.BeginOutputReadLine();
               process.BeginErrorReadLine();

               try
               {
                   process.WaitForExit();

                   Platform.FreeConsole();

                   Platform.SetConsoleCtrlHandler(null, false);
               }
               catch (Exception e)
               {
                   Console.WriteLine(e);
               }

               console.WriteLine("[GDB] - Closed");
           });

            return true;
        }

        public event EventHandler<StopRecord> Stopped;
        private event EventHandler<StopRecord> InternalStopped;

        public LiveBreakPoint SetBreakPoint(string file, uint line)
        {
            LiveBreakPoint result = null;

            SafelyExecuteCommand(() =>
            {
                result = new SetBreakPointCommand(file, line).Execute(this).Value;
            });

            return result;
        }

        protected bool SafelyExecuteCommandWithoutResume(Action commandAction)
        {
            bool result = false;

            if (CurrentState == DebuggerState.Running)
            {
                StoppedEventIsEnabled = false;

                result = Pause();

                StoppedEventIsEnabled = true;
            }

            commandAction();

            return result;
        }

        protected void SafelyExecuteCommand(Action commandAction)
        {
            if (SafelyExecuteCommandWithoutResume(commandAction))
            {
                Continue();
            }
        }

        protected void SafelyExecuteCommandAlwaysContinue(Action commandAction)
        {
            SafelyExecuteCommandWithoutResume(commandAction);

            Continue();
        }

        public void Remove(LiveBreakPoint breakPoint)
        {
            SafelyExecuteCommand(() => new RemoveBreakPointCommand(breakPoint).Execute(this));
        }

        public DebuggerState State { get { return CurrentState; } }

        public event EventHandler<EventArgs> StateChanged;

        public Dictionary<int, Register> GetRegisters()
        {
            var result = new Dictionary<int, Register>();

            List<string> registerNames = null;
            List<Tuple<int, string>> registerValues = null;

            SafelyExecuteCommand(() =>
            {
                registerNames = new DataListRegisterNamesCommand().Execute(this).Value;

                registerValues = new DataListRegisterValuesCommand().Execute(this).Value;
            });

            if (registerNames != null && registerValues != null)
            {
                for (int i = 0; i < registerNames.Count; i++)
                {
                    string name = registerNames[i];

                    if (name != string.Empty)
                    {
                        result.Add(i, new Register() { Name = name, Index = i });
                    }
                }

                foreach (var regVal in registerValues)
                {
                    if (regVal.Item1 != -1)
                    {
                        Register value;

                        if (result.TryGetValue(regVal.Item1, out value))
                        {
                            value.Value = regVal.Item2;
                        }
                    }
                }
            }

            return result;
        }

        public Dictionary<int, string> GetChangedRegisters()
        {
            var result = new Dictionary<int, string>();

            SafelyExecuteCommand(() =>
            {
                var changedIndexes = new DataListChangedRegistersCommand().Execute(this).Value;

                if (changedIndexes != null)
                {
                    var registerValues = new DataListRegisterValuesCommand(changedIndexes).Execute(this).Value;

                    foreach (var regVal in registerValues)
                    {
                        result.Add(regVal.Item1, regVal.Item2);
                    }
                }
            });

            return result;
        }

        public List<InstructionLine> Disassemble(string file, int line, int numLines)
        {
            List<InstructionLine> result = null;

            SafelyExecuteCommand(() =>
            {
                result = new DataDisassembleCommand(file, line, numLines).Execute(this).Value;
            });

            return result;
        }

        public List<InstructionLine> Disassemble(ulong start, uint count)
        {
            List<InstructionLine> result = null;

            SafelyExecuteCommand(() =>
            {
                result = new DataDisassembleCommand(start, count).Execute(this).Value;
            });

            return result;
        }

        public void Reset(bool runAfter)
        {
            SafelyExecuteCommandWithoutResume(() =>
            {
                new MonitorCommand("reset").Execute(this);

                if (runAfter)
                {
                    Continue();
                }
                else
                {
                    StepInstruction();
                }
            });
        }

        public VariableObject CreateWatch(string id, string expression)
        {
            var result = new VarCreateCommand(id, VariableObjectType.Floating, expression).Execute(this);

            if (result.Response == ResponseCode.Done)
            {
                //result.Value.Evaluate (this); // causes gdb list children and evaluate commands so that values are read.

                new VarSetFormatCommand(id, "natural").Execute(this);

                return result.Value;
            }
            else
            {
                return null;
            }
        }

        public void SetWatchFormat(string id, WatchFormat format)
        {
            string formatString = "natural";

            switch (format)
            {
                case WatchFormat.Binary:
                    formatString = "binary";
                    break;

                case WatchFormat.Decimal:
                    formatString = "decimal";
                    break;

                case WatchFormat.Hexadecimal:
                    formatString = "hexadecimal";
                    break;

                case WatchFormat.Octal:
                    formatString = "octal";
                    break;
            }

            new VarSetFormatCommand(id, formatString).Execute(this);
        }

        public void DeleteWatch(string id)
        {
            new VarDeleteCommand(id).Execute(this);
        }

        public virtual void ProvisionSettings(IProject project)
        {

        }

        public virtual UserControl GetSettingsControl(IProject project)
        {
            return new UserControl();
        }
    }
}
