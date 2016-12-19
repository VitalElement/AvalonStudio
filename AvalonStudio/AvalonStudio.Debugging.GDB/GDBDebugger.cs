using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AvalonStudio.Extensibility.Threading;
using AvalonStudio.Platforms;
using AvalonStudio.Projects;
using AvalonStudio.Toolchains;
using AvalonStudio.Toolchains.GCC;
using AvalonStudio.Utils;

namespace AvalonStudio.Debugging.GDB
{
	public class GDBDebugger : IDebugger
	{
		private CancellationTokenSource closeTokenSource;

		protected IConsole console;
        protected bool asyncModeEnabled;

		private Command currentCommand;

		private DebuggerState currentState = DebuggerState.NotRunning;

		private StreamWriter input;
		private Process process;
		private string response = string.Empty;
		private SemaphoreSlim responseReceived;

		private JobRunner transmitRunner;

		private int variableId;

		private readonly SemaphoreSlim waitForStop = new SemaphoreSlim(0, 1);

		public GDBDebugger()
		{
			StoppedEventIsEnabled = true;
            asyncModeEnabled = false;
		}

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

		[XmlIgnore]
		public bool StoppedEventIsEnabled { get; set; }

		public bool DebugMode { get; set; }

		public async Task<List<MemoryBytes>> ReadMemoryBytesAsync(ulong address, ulong offset, uint count)
		{
			List<MemoryBytes> result = null;

			await
				SafelyExecuteCommand(
					async () => { result = (await new DataReadMemoryBytesCommand(address, offset, count).Execute(this)).Value; });

			return result;
		}

		public async Task<List<VariableObjectChange>> UpdateVariablesAsync()
		{
			GDBResponse<List<VariableObjectChange>> response = null;

			response = await new VarUpdateCommand().Execute(this);

			if (response.Response == ResponseCode.Done)
			{
				return response.Value;
			}
			return null;
		}

		public async Task<string> EvaluateExpressionAsync(string expression)
		{
			if (CurrentState != DebuggerState.Paused)
			{
				throw new Exception("Expressions can only be evaluated in the paused state.");
			}

			var result = string.Empty;

			var response = await new VarEvaluateExpressionCommand(expression).Execute(this);

			if (response.Response == ResponseCode.Done)
			{
				result = response.Value;
			}

			return result;
		}

		public async Task<List<VariableObject>> ListChildrenAsync(VariableObject variable)
		{
			if (CurrentState != DebuggerState.Paused)
			{
				throw new Exception("Expressions can only be evaluated in the paused state.");
			}

			var response = await new VarListChildrenCommand(variable).Execute(this);

			if (response.Response == ResponseCode.Done)
			{
				return response.Value;
			}
			return null;
		}


		public void Initialise()
		{
			responseReceived = new SemaphoreSlim(0, 1);
		}

		public async Task<List<Variable>> ListStackVariablesAsync()
		{
			GDBResponse<List<Variable>> result = null;

			await SafelyExecuteCommand(async () => { result = await new StackListVariablesCommand().Execute(this); });

			if (result != null)
			{
				return result.Value;
			}
			return null;
		}

		public async Task<List<Frame>> ListStackFramesAsync()
		{
			GDBResponse<List<Frame>> result = null;

			await SafelyExecuteCommand(async () => { result = await new StackListFramesCommand().Execute(this); });

			if (result.Response == ResponseCode.Done)
			{
				return result.Value;
			}
			return null;
		}

		public async Task<LiveBreakPoint> BreakMainAsync()
		{
			return (await new SetBreakPointCommand("main").Execute(this)).Value;
		}

		/// <summary>
		///     This method is not supported by embedded targets. Use continue instead.
		/// </summary>
		public virtual async Task RunAsync()
		{
			if (CurrentState != DebuggerState.Running)
			{
				await new RunCommand().Execute(this);
			}
		}

		public async Task<bool> PauseAsync()
		{
			var result = true;

			if (currentState == DebuggerState.Paused)
			{
				result = false;
				return result;
			}

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

				if (waitForStop.CurrentCount == 0)
				{
					waitForStop.Release();
				}
			};

			InternalStopped += onStoppedHandler;

            if (asyncModeEnabled)
            {
                await new ExecInterruptCommand().Execute(this);

                await waitForStop.WaitAsync();
            }
            else
            {
                await transmitRunner.InvokeAsync(() =>
                {
                    do
                    {
                        Platform.SendSignal(process.Id, Platform.Signum.SIGINT);
                    } while (!waitForStop.Wait(100));
                });
            }

			InternalStopped -= onStoppedHandler;

			return result;
		}

		public async Task ContinueAsync()
		{
			if (CurrentState != DebuggerState.Running)
			{
				await new ContinueCommand().Execute(this);
			}
		}

		public async Task StepOverAsync()
		{
			await new StepOverCommand().Execute(this);
		}

		public async Task StepOutAsync()
		{
			await new FinishCommand().Execute(this);
		}

		public async Task StepInstructionAsync()
		{
			await new ExecNextInstructionCommand().Execute(this);
		}

		public async Task StepIntoAsync()
		{
			await new StepIntoCommand().Execute(this);
		}

		public virtual async Task StopAsync()
		{
			await transmitRunner.InvokeAsync(() =>
			{
				input.WriteLine("-gdb-exit");

				closeTokenSource?.Cancel();
			});
		}

		public async Task CloseAsync()
		{
            if (transmitRunner != null && process != null && !process.HasExited)
            {
                await transmitRunner.InvokeAsync(() =>
                {
                    input.WriteLine("-gdb-exit");
                    process?.WaitForExit();
                    closeTokenSource?.Cancel();
                });
            }
		}

		public virtual async Task<bool> StartAsync(IToolChain toolchain, IConsole console, IProject project)
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
				console.WriteLine("[GDB] - Error GDB is not able to debug projects compiled on this kind of toolchain (" +
				                  toolchain.GetType() + ")");
				return false;
			}

			startInfo.Arguments = string.Format("--interpreter=mi \"{0}\"",
				Path.Combine(project.CurrentDirectory, project.Executable).ToPlatformPath());

			if (Path.IsPathRooted(startInfo.FileName) && !System.IO.File.Exists(startInfo.FileName))
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

			var attempts = 0;
			while (!Platform.FreeConsole() && attempts < 10)
			{
				Console.WriteLine(Marshal.GetLastWin32Error());
				Thread.Sleep(10);
				attempts++;
			}

			attempts = 0;

			while (!Platform.AttachConsole(process.Id) && attempts < 10)
			{
				Thread.Sleep(10);
				attempts++;
			}

			while (!Platform.SetConsoleCtrlHandler(null, true))
			{
				Console.WriteLine(Marshal.GetLastWin32Error());
				Thread.Sleep(10);
			}

            TaskCompletionSource<JobRunner> transmitRunnerSet = new TaskCompletionSource<JobRunner>();

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Task.Factory.StartNew(() =>
			{
                transmitRunner = new JobRunner();

                transmitRunnerSet.SetResult(transmitRunner);

                closeTokenSource = new CancellationTokenSource();

				transmitRunner.RunLoop(closeTokenSource.Token);

				transmitRunner = null;
			});
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

            await transmitRunnerSet.Task;

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

				closeTokenSource?.Cancel();
			});

            await SafelyExecuteCommand(async () => await new EnablePrettyPrintingCommand().Execute(this));

			return true;
		}

		public event EventHandler<StopRecord> Stopped;

		public async Task<LiveBreakPoint> SetBreakPointAsync(string file, uint line)
		{
			LiveBreakPoint result = null;

			await
				SafelyExecuteCommand(async () => { result = (await new SetBreakPointCommand(file, line).Execute(this)).Value; });

			return result;
		}

		public async Task RemoveAsync(LiveBreakPoint breakPoint)
		{
			await SafelyExecuteCommand(async () => await new RemoveBreakPointCommand(breakPoint).Execute(this));
		}

		public DebuggerState State
		{
			get { return CurrentState; }
		}

		public int GetVariableId()
		{
			return variableId++;
		}

		public event EventHandler<EventArgs> StateChanged;

		public async Task<Dictionary<int, Register>> GetRegistersAsync()
		{
			var result = new Dictionary<int, Register>();

			List<string> registerNames = null;
			List<Tuple<int, string>> registerValues = null;

			await SafelyExecuteCommand(async () =>
			{
				registerNames = (await new DataListRegisterNamesCommand().Execute(this)).Value;

				registerValues = (await new DataListRegisterValuesCommand().Execute(this)).Value;
			});

			if (registerNames != null && registerValues != null)
			{
				for (var i = 0; i < registerNames.Count; i++)
				{
					var name = registerNames[i];

					if (name != string.Empty)
					{
						result.Add(i, new Register {Name = name, Index = i});
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

		public async Task<Dictionary<int, string>> GetChangedRegistersAsync()
		{
			var result = new Dictionary<int, string>();

			await SafelyExecuteCommand(async () =>
			{
				var changedIndexes = (await new DataListChangedRegistersCommand().Execute(this)).Value;

				if (changedIndexes != null)
				{
					var registerValues = (await new DataListRegisterValuesCommand(changedIndexes).Execute(this)).Value;

					foreach (var regVal in registerValues)
					{
						result.Add(regVal.Item1, regVal.Item2);
					}
				}
			});

			return result;
		}

		public async Task<List<InstructionLine>> DisassembleAsync(string file, int line, int numLines)
		{
			List<InstructionLine> result = null;

			await
				SafelyExecuteCommand(
					async () => { result = (await new DataDisassembleCommand(file, line, numLines).Execute(this)).Value; });

			return result;
		}

		public async Task<List<InstructionLine>> DisassembleAsync(ulong start, uint count)
		{
			List<InstructionLine> result = null;

			await
				SafelyExecuteCommand(async () => { result = (await new DataDisassembleCommand(start, count).Execute(this)).Value; });

			return result;
		}

		public virtual async Task ResetAsync(bool runAfter)
		{
			await SafelyExecuteCommandWithoutResume(async () =>
			{
				await new MonitorCommand("reset").Execute(this);

				if (runAfter)
				{
					await ContinueAsync();
				}
				else
				{
					await StepInstructionAsync();
				}
			});
		}

		public async Task<VariableObject> CreateWatchAsync(string id, string expression)
		{
			var result = await new VarCreateCommand(id, VariableObjectType.Floating, expression).Execute(this);

			if (result.Response == ResponseCode.Done)
			{
				//result.Value.Evaluate (this); // causes gdb list children and evaluate commands so that values are read.

				await new VarSetFormatCommand(id, "natural").Execute(this);

				return result.Value;
			}
			return null;
		}

		public async Task SetWatchFormatAsync(string id, WatchFormat format)
		{
			var formatString = "natural";

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

			await new VarSetFormatCommand(id, formatString).Execute(this);
		}

		public async Task DeleteWatchAsync(string id)
		{
			await new VarDeleteCommand(id).Execute(this);
		}

		public virtual void ProvisionSettings(IProject project)
		{
		}

		public virtual object GetSettingsControl(IProject project)
		{
			return new object();
		}

		~GDBDebugger()
		{
			if (process != null && !process.HasExited)
			{
				process.Kill();
			}
		}

		internal Command SetCommand(Command command)
		{
			if (currentCommand != null)
			{
				console.WriteLine("Serious error.");
			}

			currentCommand = command;
			return command;
		}

		internal void ClearCommand()
		{
			currentCommand = null;
		}

		private string WaitForResponse(int timeout)
		{
			var result = string.Empty;

			if (responseReceived.Wait(timeout))
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

		internal async Task<string> SendCommand(Command command, int timeout)
		{
			var result = string.Empty;

			await transmitRunner.InvokeAsync(() =>
			{
				SetCommand(command);

				if (DebugMode)
				{
					console.WriteLine("[Sending] " + command.Encode());
				}

				input.WriteLine(command.Encode());
				input.Flush();

				result = WaitForResponse(timeout);

				ClearCommand();
			});

			return result;
		}

		private void ProcessOutput(string data)
		{
			switch (data[0])
			{
				case '*': // out of band data.
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

				case '=': // notification record
					if (DebugMode)
					{
						console.WriteLine("[Notification] " + data);
					}
					break;

				case '^': //this is for a response.
					if (DebugMode)
					{
						console.WriteLine("[Response] " + data + Environment.NewLine);
					}

					if (currentCommand != null)
					{
						response += data;

						if (responseReceived.CurrentCount == 0)
						{
							responseReceived.Release();
						}
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
			var split = data.Split(new[] {','}, 2);

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
				Task.Factory.StartNew(() => { Stopped(this, stopRecord); });
			}
		}

		private event EventHandler<StopRecord> InternalStopped;

		protected async Task<bool> SafelyExecuteCommandWithoutResume(Func<Task> commandAction)
		{
			var result = false;

			if (CurrentState == DebuggerState.Running)
			{
				StoppedEventIsEnabled = false;

				result = await PauseAsync();

				StoppedEventIsEnabled = true;
			}

			await commandAction();

			return result;
		}

		protected async Task SafelyExecuteCommand(Func<Task> commandAction)
		{
			if (await SafelyExecuteCommandWithoutResume(commandAction))
			{
				await ContinueAsync();
			}
		}

		protected async void SafelyExecuteCommandAlwaysContinue(Func<Task> commandAction)
		{
			await SafelyExecuteCommandWithoutResume(commandAction);

			await ContinueAsync();
		}
	}
}