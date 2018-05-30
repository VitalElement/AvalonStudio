using AvalonStudio.Commands;
using AvalonStudio.Extensibility;
using System.Composition;
using ReactiveUI;

namespace AvalonStudio.Debugging.Commands
{
    internal class DebugCommands
    {
        [ExportCommandDefinition("Debug.Start")]
        [DefaultKeyGestures("F5")]
        public CommandDefinition StartDebuggingCommand { get; }

        [ExportCommandDefinition("Debug.Pause")]
        public CommandDefinition PauseDebuggingCommand { get; }

        [ExportCommandDefinition("Debug.Stop")]
        public CommandDefinition StopDebuggingCommand { get; }

        [ExportCommandDefinition("Debug.Restart")]
        public CommandDefinition RestartDebuggingCommand { get; }

        [ExportCommandDefinition("Debug.StepOver")]
        [DefaultKeyGestures("F10")]
        public CommandDefinition StepOverCommand { get; }

        [ExportCommandDefinition("Debug.StepInto")]
        [DefaultKeyGestures("F11")]
        public CommandDefinition StepIntoCommand { get; }

        [ExportCommandDefinition("Debug.StepOut")]
        [DefaultKeyGestures("SHIFT+F11")]
        public CommandDefinition StepOutCommand { get; }

        [ExportCommandDefinition("Debug.StepInstruction")]
        [DefaultKeyGestures("F9")]
        public CommandDefinition StepInstructionCommand { get; }

        private IDebugManager2 _debugManager;

        [ImportingConstructor]
        public DebugCommands(CommandIconService commandIconService)
        {
            _debugManager = IoC.Get<IDebugManager2>();

            StartDebuggingCommand = new CommandDefinition(
                "Start",
                commandIconService.GetCompletionKindImage("Run"),
                ReactiveCommand.Create(StartDebugging, _debugManager.CanStart));

            PauseDebuggingCommand = new CommandDefinition(
                "Pause",
                commandIconService.GetCompletionKindImage("PauseDebugger"),
                ReactiveCommand.Create(_debugManager.Pause, _debugManager.CanPause));

            StopDebuggingCommand = new CommandDefinition(
                "Stop",
                commandIconService.GetCompletionKindImage("Stop"),
                ReactiveCommand.Create(_debugManager.Stop, _debugManager.CanStop));

            RestartDebuggingCommand = new CommandDefinition(
                "Restart",
                commandIconService.GetCompletionKindImage("Restart"),
                ReactiveCommand.Create(_debugManager.Restart, _debugManager.CanStop));

            StepOverCommand = new CommandDefinition(
                "Step Over",
                commandIconService.GetCompletionKindImage("StepOver"),
                ReactiveCommand.Create(_debugManager.StepOver, _debugManager.CanStep)); ;

            StepIntoCommand = new CommandDefinition(
                "Step Into",
                commandIconService.GetCompletionKindImage("StepInto"),
                ReactiveCommand.Create(_debugManager.StepInto, _debugManager.CanStep)); ;

            StepOutCommand = new CommandDefinition(
                "Step Out",
                commandIconService.GetCompletionKindImage("StepOut"),
                ReactiveCommand.Create(_debugManager.StepOut, _debugManager.CanStep));

            StepInstructionCommand = new CommandDefinition(
                "Step Instruction",
                null,
                ReactiveCommand.Create(_debugManager.StepInstruction, _debugManager.CanStep));
        }

        private void StartDebugging()
        {
            if (_debugManager.SessionActive)
            {
                _debugManager.Continue();
            }
            else
            {
                _debugManager.Start();
            }
        }
    }
}
