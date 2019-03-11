// GdbSession.cs
//
// Author:
//   Lluis Sanchez Gual <lluis@novell.com>
//
// Copyright (c) 2008 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.Platforms;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using Mono.Debugging.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging.GDB
{
    public class GdbSession : ExtendedDebuggerSession
    {
        private IConsole _console;
        private Process proc;
        private CancellationTokenSource closeTokenSource;

        private StreamReader sout;
        private StreamWriter sin;
        private GdbCommandResult lastResult;
        private bool running;
        private long currentThread = -1;
        private long activeThread = -1;
        private bool isMonoProcess;
        private string currentProcessName;
        private Dictionary<string, WeakReference<ObjectValue>> tempVariableObjects = new Dictionary<string, WeakReference<ObjectValue>>();
        private Dictionary<int, BreakEventInfo> breakpoints = new Dictionary<int, BreakEventInfo>();
        private List<BreakEventInfo> breakpointsWithHitCount = new List<BreakEventInfo>();

        private DateTime lastBreakEventUpdate = DateTime.Now;
        private Dictionary<int, WaitCallback> breakUpdates = new Dictionary<int, WaitCallback>();
        private bool breakUpdateEventsQueued;
        private const int BreakEventUpdateNotifyDelay = 500;

        private bool internalStop;
        private bool logGdb = false;
        private bool asyncMode;
        private bool _detectAsync;

        private object syncLock = new object();
        private object eventLock = new object();
        private object gdbLock = new object();

        private string _gdbExecutable;
        private string _runCommand;
        private bool _suppressEvents = false;
        private bool _waitForStopBeforeRunning;

        /// <summary>
        /// Override if the GDBSession should Load Symbols (-file-exec-and-symbols command) before a session starts).
        /// </summary>
        protected virtual bool ManuallyLoadSymbols => false;

        /// <summary>
		/// Raised when the debugging session is paused
		/// </summary>
		public event EventHandler<TargetEventArgs> TargetStoppedWhenSuppressed;

        public GdbSession(string gdbExecutable, string runCommand = "-exec-run", bool detectAsync = true, bool waitForStopBeforeRunning = false)
        {
            _gdbExecutable = gdbExecutable;
            _console = IoC.Get<IConsole>();
            _runCommand = runCommand;
            _detectAsync = detectAsync;
            _waitForStopBeforeRunning = waitForStopBeforeRunning;

            logGdb = IoC.Get<IStudio>().DebugMode;
        }

        protected override void OnRun(DebuggerStartInfo startInfo)
        {
            lock (gdbLock)
            {
                // Create a script to be run in a terminal
                string script = Path.GetTempFileName();
                string ttyfile = Path.GetTempFileName();
                string ttyfileDone = ttyfile + "_done";
                string tty = string.Empty;

                if (File.Exists(_gdbExecutable))
                {
                    StartGdb(startInfo);

                    // Initialize the terminal
                    RunCommand("-inferior-tty-set", Escape(tty));

                    if (ManuallyLoadSymbols)
                    {
                        try
                        {
                            RunCommand("-file-exec-and-symbols", Escape(startInfo.Command.ToAvalonPath()));
                        }
                        catch
                        {
                            FireTargetEvent(TargetEventType.TargetExited, null);
                            throw;
                        }
                    }

                    RunCommand("-environment-cd", Escape(startInfo.WorkingDirectory));

                    // Set inferior arguments
                    if (!string.IsNullOrEmpty(startInfo.Arguments))
                        RunCommand("-exec-arguments", startInfo.Arguments);

                    if (startInfo.EnvironmentVariables != null)
                    {
                        foreach (var v in startInfo.EnvironmentVariables)
                            RunCommand("-gdb-set", "environment", v.Key, v.Value);
                    }

                    currentProcessName = startInfo.Command + " " + startInfo.Arguments;

                    if (_detectAsync)
                    {
                        asyncMode = RunCommand("-gdb-set", "mi-async", "on").Status == CommandStatus.Done;
                    }
                    else
                    {
                        asyncMode = false;
                    }

                    if (!asyncMode && Platform.PlatformIdentifier == AvalonStudio.Platforms.PlatformID.Win32NT)
                    {
                        // TODO check if this code can be removed, it was used to support  ctrl+c signals, but no longer seems
                        // to be needed for .net core.
                        var attempts = 0;
                        while (!Platform.FreeConsole() && attempts < 10)
                        {
                            _console.WriteLine(Marshal.GetLastWin32Error().ToString());
                            Thread.Sleep(10);
                            attempts++;
                        }

                        attempts = 0;

                        while (!Platform.AttachConsole(proc.Id) && attempts < 10)
                        {
                            Thread.Sleep(10);
                            attempts++;
                        }

                        while (!Platform.SetConsoleCtrlHandler(null, true))
                        {
                            _console.WriteLine(Marshal.GetLastWin32Error().ToString());
                            Thread.Sleep(10);
                        }
                    }

                    if (Platform.PlatformIdentifier == Platforms.PlatformID.Win32NT)
                    {
                        RunCommand("-gdb-set", "new-console", "on");
                    }

                    RunCommand("-enable-pretty-printing");

                    OnStarted();

                    if (!startInfo.RequiresManualStart)
                    {
                        if (_waitForStopBeforeRunning)
                        {
                            _suppressEvents = true;

                            var catchFirstStop = Observable.FromEventPattern(this, nameof(TargetStoppedWhenSuppressed)).Take(1).Subscribe(s =>
                            {
                                ThreadPool.QueueUserWorkItem(delegate
                                {
                                    _suppressEvents = false;
                                    running = true;
                                    RunCommand(_runCommand);
                                });
                            });
                        }
                        else
                        {
                            running = true;
                            RunCommand(_runCommand);
                        }
                    }
                    else
                    {
                        running = false;
                    }
                }
                else
                {
                    running = false;
                }
            }
        }

        protected override void OnAttachToProcess(long processId)
        {
            lock (gdbLock)
            {
                //StartGdb();
                currentProcessName = "PID " + processId.ToString();
                RunCommand("attach", processId.ToString());
                currentThread = activeThread = 1;
                CheckIsMonoProcess();
                OnStarted();
                FireTargetEvent(TargetEventType.TargetStopped, null);
            }
        }

        public bool IsMonoProcess
        {
            get { return isMonoProcess; }
        }

        private void CheckIsMonoProcess()
        {
            try
            {
                RunCommand("-data-evaluate-expression", "mono_pmip");
                isMonoProcess = true;
            }
            catch
            {
                isMonoProcess = false;
                // Ignore
            }
        }

        private void StartGdb(DebuggerStartInfo startinfo)
        {
            proc = new Process();
            proc.StartInfo.FileName = _gdbExecutable;
            proc.StartInfo.Arguments = "--interpreter=mi " + startinfo.Command;
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardInput = true;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.RedirectStandardError = true;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();

            sout = proc.StandardOutput;
            sin = proc.StandardInput;

            closeTokenSource = new CancellationTokenSource();
            Task.Factory.StartNew(OutputInterpreter, closeTokenSource.Token);
        }

        public override void Dispose()
        {
            /*if (console != null && !console.IsCompleted) {
				console.Cancel ();
				console = null;
			}*/

            closeTokenSource?.Cancel();

            if (!asyncMode && Platform.PlatformIdentifier == AvalonStudio.Platforms.PlatformID.Win32NT)
            {
                Platform.FreeConsole();

                Platform.SetConsoleCtrlHandler(null, false);
            }
        }

        protected override void OnSetActiveThread(long processId, long threadId)
        {
            activeThread = threadId;
        }

        protected override void OnStop()
        {
            if (asyncMode)
            {
                RunCommand("-exec-interrupt");
            }
            else
            {
                lock (eventLock)
                {
                    do
                    {
                        Platform.SendSignal(proc.Id, Platform.Signum.SIGINT);
                    }
                    while (!Monitor.Wait(eventLock, 100));
                }
            }
        }

        protected override void OnDetach()
        {
            lock (gdbLock)
            {
                InternalStop();
                RunCommand("detach");
                FireTargetEvent(TargetEventType.TargetExited, null);
            }
        }

        protected override void OnExit()
        {
            lock (gdbLock)
            {
                InternalStop();

                try
                {
                    sin?.WriteLine("-gdb-exit");
                }
                catch(System.IO.IOException)
                {

                }
                
                closeTokenSource?.Cancel();
                TargetEventArgs args = new TargetEventArgs(TargetEventType.TargetExited);
                OnTargetEvent(args);
            }
        }

        protected override void OnStepLine()
        {
            SelectThread(activeThread);
            RunCommand("-exec-step");
        }

        protected override void OnNextLine()
        {
            SelectThread(activeThread);
            RunCommand("-exec-next");
        }

        protected override void OnStepInstruction()
        {
            SelectThread(activeThread);
            RunCommand("-exec-step-instruction");
        }

        protected override void OnNextInstruction()
        {
            SelectThread(activeThread);
            RunCommand("-exec-next-instruction");
        }

        protected override void OnFinish()
        {
            SelectThread(activeThread);
            GdbCommandResult res = RunCommand("-stack-info-depth", "2");
            if (res.GetValue("depth") == "1")
            {
                RunCommand("-exec-continue");
            }
            else
            {
                RunCommand("-stack-select-frame", "0");
                RunCommand("-exec-finish");
            }
        }

        private BreakEventInfo OnInsertBreakPoint(BreakEvent be)
        {
            var bi = new BreakEventInfo();
            var bp = be as Breakpoint;

            lock (gdbLock)
            {
                bool dres = InternalStop();
                try
                {
                    string extraCmd = string.Empty;
                    if (bp.HitCount > 0)
                    {
                        extraCmd += "-i " + bp.HitCount;
                        breakpointsWithHitCount.Add(bi);
                    }
                    if (!string.IsNullOrEmpty(bp.ConditionExpression))
                    {
                        if (!bp.BreakIfConditionChanges)
                            extraCmd += " -c " + bp.ConditionExpression;
                    }

                    GdbCommandResult res = null;
                    string errorMsg = null;

                    if (bp is FunctionBreakpoint)
                    {
                        try
                        {
                            res = RunCommand("-break-insert", extraCmd.Trim(), ((FunctionBreakpoint)bp).FunctionName);
                        }
                        catch (Exception ex)
                        {
                            errorMsg = ex.Message;
                        }
                    }
                    else
                    {
                        // Breakpoint locations must be double-quoted if files contain spaces.
                        // For example: -break-insert "\"C:/Documents and Settings/foo.c\":17"
                        RunCommand("-environment-directory", Escape(Path.GetDirectoryName(bp.FileName).ToAvalonPath()));

                        try
                        {
                            res = RunCommand("-break-insert", extraCmd.Trim(), Escape(Escape(bp.FileName.ToAvalonPath()) + ":" + bp.Line));
                        }
                        catch (Exception ex)
                        {
                            errorMsg = ex.Message;
                        }

                        if (res == null)
                        {
                            try
                            {
                                res = RunCommand("-break-insert", extraCmd.Trim(), Escape(Escape(Path.GetFileName(bp.FileName)) + ":" + bp.Line));
                            }
                            catch
                            {
                                // Ignore
                            }
                        }
                    }

                    if (res == null || res.Status != CommandStatus.Done)
                    {
                        bi.SetStatus(BreakEventStatus.Invalid, errorMsg);
                        return bi;
                    }
                    int bh = res.GetObject("bkpt").GetInt("number");
                    if (!be.Enabled)
                        RunCommand("-break-disable", bh.ToString());
                    breakpoints[bh] = bi;
                    bi.Handle = bh;
                    bi.SetStatus(BreakEventStatus.Bound, null);
                    return bi;
                }
                finally
                {
                    InternalResume(dres);
                }
            }
        }

        private BreakEventInfo OnInsertWatchPoint(BreakEvent be)
        {
            var bi = new BreakEventInfo();
            var wp = be as WatchPoint;

            lock (gdbLock)
            {
                bool dres = InternalStop();
                try
                {
                    string errorMsg = string.Empty;
                    GdbCommandResult res = null;

                    try
                    {
                        if (wp.TriggerOnRead && wp.TriggerOnWrite)
                        {
                            res = RunCommand("-break-watch", wp.Expression, "-a");
                        }
                        else if (wp.TriggerOnRead && !wp.TriggerOnWrite)
                        {
                            res = RunCommand("-break-watch", wp.Expression, "-r");
                        }
                        else
                        {
                            res = RunCommand("-break-watch", wp.Expression);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMsg = ex.Message;
                    }

                    if (res == null || res.Status != CommandStatus.Done)
                    {
                        bi.SetStatus(BreakEventStatus.Invalid, errorMsg);
                        return bi;
                    }

                    int bh = res.GetObject("wpt").GetInt("number");

                    if (!be.Enabled)
                    {
                        RunCommand("-break-disable", bh.ToString());
                    }

                    breakpoints[bh] = bi;
                    bi.Handle = bh;
                    bi.SetStatus(BreakEventStatus.Bound, null);
                    return bi;
                }
                finally
                {
                    InternalResume(dres);
                }
            }
        }

        protected override BreakEventInfo OnInsertBreakEvent(BreakEvent be)
        {
            if (be is Breakpoint)
            {
                return OnInsertBreakPoint(be as Breakpoint);
            }
            else if (be is WatchPoint)
            {
                return OnInsertWatchPoint(be as WatchPoint);
            }
            else
            {
                var bi = new BreakEventInfo();

                bi.SetStatus(BreakEventStatus.NotBound, "BreakEvent type not supported.");

                return bi;
            }
        }

        private bool CheckBreakpoint(int handle)
        {
            BreakEventInfo binfo;
            if (!breakpoints.TryGetValue(handle, out binfo))
                return true;

            Breakpoint bp = (Breakpoint)binfo.BreakEvent;

            if (!string.IsNullOrEmpty(bp.ConditionExpression) && bp.BreakIfConditionChanges)
            {
                // Update the condition expression
                GdbCommandResult res = RunCommand("-data-evaluate-expression", Escape(bp.ConditionExpression));
                string val = res.GetValue("value");
                RunCommand("-break-condition", handle.ToString(), "(" + bp.ConditionExpression + ") != " + val);
            }

            if (!string.IsNullOrEmpty(bp.TraceExpression) && bp.HitAction == HitAction.PrintExpression)
            {
                GdbCommandResult res = RunCommand("-data-evaluate-expression", Escape(bp.TraceExpression));
                string val = res.GetValue("value");
                NotifyBreakEventUpdate(binfo, 0, val);
                return false;
            }
            return true;
        }

        private void NotifyBreakEventUpdate(BreakEventInfo binfo, int hitCount, string lastTrace)
        {
            bool notify = false;

            WaitCallback nc = delegate
            {
                if (hitCount != -1)
                    binfo.IncrementHitCount();
                if (lastTrace != null)
                    binfo.UpdateLastTraceValue(lastTrace);
            };

            lock (breakUpdates)
            {
                int span = (int)(DateTime.Now - lastBreakEventUpdate).TotalMilliseconds;
                if (span >= BreakEventUpdateNotifyDelay && !breakUpdateEventsQueued)
                {
                    // Last update was more than 0.5s ago. The update can be sent.
                    lastBreakEventUpdate = DateTime.Now;
                    notify = true;
                }
                else
                {
                    // Queue the event notifications to avoid wasting too much time
                    breakUpdates[(int)binfo.Handle] = nc;
                    if (!breakUpdateEventsQueued)
                    {
                        breakUpdateEventsQueued = true;

                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            Thread.Sleep(BreakEventUpdateNotifyDelay - span);
                            List<WaitCallback> copy;
                            lock (breakUpdates)
                            {
                                copy = new List<WaitCallback>(breakUpdates.Values);
                                breakUpdates.Clear();
                                breakUpdateEventsQueued = false;
                                lastBreakEventUpdate = DateTime.Now;
                            }
                            foreach (WaitCallback wc in copy)
                                wc(null);
                        });
                    }
                }
            }
            if (notify)
                nc(null);
        }

        private void UpdateHitCountData()
        {
            foreach (BreakEventInfo bp in breakpointsWithHitCount)
            {
                GdbCommandResult res = RunCommand("-break-info", bp.Handle.ToString());
                string val = res.GetObject("BreakpointTable").GetObject("body").GetObject(0).GetObject("bkpt").GetValue("ignore");
                if (val != null)
                    NotifyBreakEventUpdate(bp, int.Parse(val), null);
                else
                    NotifyBreakEventUpdate(bp, 0, null);
            }
            breakpointsWithHitCount.Clear();
        }

        protected override void OnRemoveBreakEvent(BreakEventInfo binfo)
        {
            lock (gdbLock)
            {
                if (binfo.Handle == null)
                    return;
                bool dres = InternalStop();
                breakpointsWithHitCount.Remove(binfo);
                breakpoints.Remove((int)binfo.Handle);
                try
                {
                    RunCommand("-break-delete", binfo.Handle.ToString());
                }
                finally
                {
                    InternalResume(dres);
                }
            }
        }

        protected override void OnEnableBreakEvent(BreakEventInfo binfo, bool enable)
        {
            lock (gdbLock)
            {
                if (binfo.Handle == null)
                    return;
                bool dres = InternalStop();
                try
                {
                    if (enable)
                        RunCommand("-break-enable", binfo.Handle.ToString());
                    else
                        RunCommand("-break-disable", binfo.Handle.ToString());
                }
                finally
                {
                    InternalResume(dres);
                }
            }
        }

        protected override void OnUpdateBreakEvent(BreakEventInfo binfo)
        {
            Breakpoint bp = binfo.BreakEvent as Breakpoint;
            if (bp == null)
                throw new NotSupportedException();

            if (binfo.Handle == null)
                return;

            bool ss = InternalStop();

            try
            {
                if (bp.HitCount > 0)
                {
                    RunCommand("-break-after", binfo.Handle.ToString(), bp.HitCount.ToString());
                    breakpointsWithHitCount.Add(binfo);
                }
                else
                    breakpointsWithHitCount.Remove(binfo);

                if (!string.IsNullOrEmpty(bp.ConditionExpression) && !bp.BreakIfConditionChanges)
                    RunCommand("-break-condition", binfo.Handle.ToString(), bp.ConditionExpression);
                else
                    RunCommand("-break-condition", binfo.Handle.ToString());
            }
            finally
            {
                InternalResume(ss);
            }
        }

        protected override void OnContinue()
        {
            SelectThread(activeThread);
            RunCommand("-exec-continue");
        }

        protected override ThreadInfo[] OnGetThreads(long processId)
        {
            List<ThreadInfo> list = new List<ThreadInfo>();
            ResultData data = RunCommand("-thread-list-ids").GetObject("thread-ids");
            foreach (string id in data.GetAllValues("thread-id"))
                list.Add(GetThread(long.Parse(id)));
            return list.ToArray();
        }

        protected override ProcessInfo[] OnGetProcesses()
        {
            ProcessInfo p = new ProcessInfo(0, currentProcessName);
            return new ProcessInfo[] { p };
        }

        private ThreadInfo GetThread(long id)
        {
            return new ThreadInfo(0, id, "Thread #" + id, null);
        }

        protected override Backtrace OnGetThreadBacktrace(long processId, long threadId)
        {
            ResultData data = SelectThread(threadId);
            GdbCommandResult res = RunCommand("-stack-info-depth");
            int fcount = int.Parse(res.GetValue("depth"));
            GdbBacktrace bt = new GdbBacktrace(this, threadId, fcount, data != null ? data.GetObject("frame") : null);
            return new Backtrace(bt);
        }

        protected override Dictionary<int, string> OnGetRegisterChanges()
        {
            var result = new Dictionary<int, string>();

            var data = RunCommand("-data-list-changed-registers");

            var indexes = data.GetObject("changed-registers");

            string indexParams = string.Empty;

            if (indexes != null)
            {
                for (int i = 0; i < indexes.Count; i++)
                {
                    indexParams += indexes.GetValue(i) + " ";
                }
            }

            var values = RunCommand("-data-list-register-values", "x", indexParams);

            var regValues = values.GetObject("register-values");

            if (regValues != null)
            {
                for (int n = 0; n < regValues.Count; n++)
                {
                    var valueObj = regValues.GetObject(n);

                    var index = valueObj.GetInt("number");
                    var value = valueObj.GetValue("value");

                    result.Add(index, value);
                }
            }

            return result;
        }

        protected override List<Register> OnGetRegisters()
        {
            var result = new List<Register>();

            var data = RunCommand("-data-list-register-names");

            if (data.Status == CommandStatus.Done)
            {
                var regNames = data.GetObject("register-names");

                for (int n = 0; n < regNames.Count; n++)
                {
                    result.Add(new Register() { Name = regNames.GetValue(n), Index = n });
                }

                data = RunCommand("-data-list-register-values", "x");

                if (data.Status == CommandStatus.Done)
                {
                    var regValues = data.GetObject("register-values");

                    for (int n = 0; n < regValues.Count; n++)
                    {
                        var valueObj = regValues.GetObject(n);

                        var index = valueObj.GetInt("number");
                        var value = valueObj.GetValue("value");

                        result[index].Value = value;
                    }
                }
            }

            return result;
        }

        protected override AssemblyLine[] OnDisassembleFile(string file)
        {
            List<AssemblyLine> lines = new List<AssemblyLine>();
            int cline = 1;
            do
            {
                GdbCommandResult data = null;
                try
                {
                    data = RunCommand("-data-disassemble", "-f", file, "-l", cline.ToString(), "--", "1");
                }
                catch
                {
                    break;
                }

                int newLine = cline;

                if (data.Status == CommandStatus.Done)
                {
                    ResultData asm_insns = data.GetObject("asm_insns");

                    for (int n = 0; n < asm_insns.Count; n++)
                    {
                        ResultData src_and_asm_line = asm_insns.GetObject(n).GetObject("src_and_asm_line");
                        newLine = src_and_asm_line.GetInt("line");
                        ResultData line_asm_insn = src_and_asm_line.GetObject("line_asm_insn");
                        for (int i = 0; i < line_asm_insn.Count; i++)
                        {
                            ResultData asm = line_asm_insn.GetObject(i);
                            long addr = long.Parse(asm.GetValue("address").Substring(2), NumberStyles.HexNumber);
                            string code = asm.GetValue("inst");
                            lines.Add(new AssemblyLine(addr, code, newLine));
                        }
                    }
                }

                if (newLine <= cline)
                    break;

                cline = newLine + 1;
            }
            while (true);

            return lines.ToArray();
        }

        public ResultData SelectThread(long id)
        {
            if (id == currentThread)
                return null;
            currentThread = id;
            return RunCommand("-thread-select", id.ToString());
        }

        private string Escape(string str)
        {
            if (str == null)
                return null;
            else if (str.IndexOf(' ') != -1 || str.IndexOf('"') != -1)
            {
                str = str.Replace("\"", "\\\"");
                return "\"" + str + "\"";
            }
            else
                return str;
        }

        public GdbCommandResult RunCommand(string command, params string[] args)
        {
            return RunCommand(command, 30000, args);
        }

        public GdbCommandResult RunCommand(string command, int timeout = 30000, params string[] args)
        {
            lock (gdbLock)
            {
                lock (syncLock)
                {
                    lastResult = null;

                    lock (eventLock)
                    {
                        running = true;
                    }

                    if (logGdb)
                        _console.WriteLine("gdb<: " + command + " " + string.Join(" ", args));

                    sin.WriteLine(command + " " + string.Join(" ", args));

                    if (!Monitor.Wait(syncLock, timeout))
                    {
                        lastResult = new GdbCommandResult("");
                        lastResult.Status = CommandStatus.Timeout;
                        throw new TimeoutException();
                    }

                    return lastResult;
                }
            }
        }

        protected bool InsideStop()
        {
            lock (gdbLock)
            {
                return InternalStop();
            }
        }

        protected void InsideResume(bool resume)
        {
            lock (gdbLock)
            {
                InternalResume(resume);
            }
        }

        private bool InternalStop()
        {
            if (!running)
                return false;
            internalStop = true;

            if (asyncMode)
            {
                lock (eventLock)
                {
                    sin.WriteLine("-exec-interrupt");

                    Monitor.Wait(eventLock);
                }
            }
            else
            {
                lock (eventLock)
                {
                    do
                    {
                        Platform.SendSignal(proc.Id, Platform.Signum.SIGINT);
                    }
                    while (!Monitor.Wait(eventLock, 100));
                }
            }

            return true;
        }

        private void InternalResume(bool resume)
        {
            if (resume)
                RunCommand("-exec-continue");
        }

        private void OutputInterpreter()
        {
            string line;
            while ((line = sout.ReadLine()) != null)
            {
                try
                {
                    ProcessOutput(line);
                }
                catch (Exception ex)
                {
                    _console.WriteLine(ex.ToString());
                }
            }
        }

        private void ProcessOutput(string line)
        {
            if (logGdb)
                _console.WriteLine("dbg>: '" + line + "'");
            switch (line[0])
            {
                case '^':
                    lock (syncLock)
                    {
                        lastResult = new GdbCommandResult(line);

                        lock (eventLock)
                        {
                            running = lastResult.Status == CommandStatus.Running;
                        }

                        Monitor.PulseAll(syncLock);
                    }
                    break;

                case '~':
                case '&':
                    if (line.Length > 1 && line[1] == '"')
                        line = line.Substring(2, line.Length - 5);
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        OnTargetOutput(false, line + "\n");
                    });
                    break;

                case '*':
                    GdbEvent ev = null;
                    lock (eventLock)
                    {
                        if (!line.StartsWith("*running"))
                        {
                            running = false;
                            ev = new GdbEvent(line);
                            string ti = ev.GetValue("thread-id");
                            if (ti != null && ti != "all")
                                currentThread = activeThread = int.Parse(ti);
                            Monitor.PulseAll(eventLock);
                            if (internalStop)
                            {
                                internalStop = false;
                                return;
                            }
                        }
                    }

                    if (ev != null)
                    {
                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            try
                            {
                                HandleEvent(ev);
                            }
                            catch (Exception ex)
                            {
                                _console.WriteLine(ex.ToString());
                            }
                        });
                    }
                    break;
            }
        }

        private void HandleEvent(GdbEvent ev)
        {
            if (ev.Name != "stopped")
            {
                return;
            }

            CleanTempVariableObjects();

            BreakEvent breakEvent = null;

            TargetEventType type = TargetEventType.TargetStopped;

            if (!string.IsNullOrEmpty(ev.Reason))
            {
                switch (ev.Reason)
                {
                    case "breakpoint-hit":
                        type = TargetEventType.TargetHitBreakpoint;
                        var breakPointNumber = ev.GetInt("bkptno");
                        if (!CheckBreakpoint(breakPointNumber))
                        {
                            RunCommand("-exec-continue");
                            return;
                        }

                        breakEvent = breakpoints[breakPointNumber].BreakEvent;
                        break;

                    case "watchpoint-trigger":
                        type = TargetEventType.TargetHitBreakpoint;

                        var watchPointNumber = ev.GetObject("wpt").GetInt("number");
                        breakEvent = breakpoints[watchPointNumber].BreakEvent;
                        break;

                    case "signal-received":
                        if (ev.GetValue("signal-name") == "SIGINT")
                            type = TargetEventType.TargetInterrupted;
                        else
                            type = TargetEventType.TargetSignaled;
                        break;

                    case "exited":
                    case "exited-signalled":
                    case "exited-normally":
                        type = TargetEventType.TargetExited;
                        break;

                    default:
                        type = TargetEventType.TargetStopped;
                        break;
                }
            }

            ResultData curFrame = ev.GetObject("frame");
            FireTargetEvent(type, curFrame, breakEvent);
        }

        private void FireTargetEvent(TargetEventType type, ResultData curFrame, BreakEvent breakEvent = null)
        {
            UpdateHitCountData();

            TargetEventArgs args = new TargetEventArgs(type);

            if (type != TargetEventType.TargetExited)
            {
                GdbCommandResult res = RunCommand("-stack-info-depth");
                int fcount = int.Parse(res.GetValue("depth"));

                GdbBacktrace bt = new GdbBacktrace(this, activeThread, fcount, curFrame);
                args.Backtrace = new Backtrace(bt);
                args.Thread = GetThread(activeThread);
                args.BreakEvent = breakEvent;
            }

            if (_suppressEvents && type == TargetEventType.TargetStopped)
            {
                args.IsStopEvent = true;
                TargetStoppedWhenSuppressed?.Invoke(this, args);
            }
            else
            {
                OnTargetEvent(args);
            }
        }

        internal void RegisterTempVariableObject(string id, ObjectValue var)
        {
            lock (tempVariableObjects)
            {
                tempVariableObjects.Add(id, new WeakReference<ObjectValue>(var));
            }
        }

        private void CleanTempVariableObjects()
        {
            lock (tempVariableObjects)
            {
                List<string> keysToRemove = new List<string>();

                foreach (var item in tempVariableObjects)
                {
                    if (!item.Value.TryGetTarget(out ObjectValue result))
                    {
                        RunCommand("-var-delete", item.Key);

                        keysToRemove.Add(item.Key);
                    }
                }

                foreach (var key in keysToRemove)
                {
                    tempVariableObjects.Remove(key);
                }
            }
        }
    }
}