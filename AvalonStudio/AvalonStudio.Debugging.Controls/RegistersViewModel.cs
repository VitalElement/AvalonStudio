using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Shell;
using AvalonStudio.Extensibility.Studio;
using AvalonStudio.MVVM;
using AvalonStudio.Shell;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Composition;
using System.Linq;
using System.Reactive.Linq;

namespace AvalonStudio.Debugging
{
    [ExportToolControl, Export(typeof(IExtension)), Shared]
    public class RegistersViewModel : ToolViewModel<ObservableCollection<RegisterViewModel>>, IActivatableExtension
    {
        private IDebugManager2 _debugManager;

        private double columnWidth;

        private bool _enabled;

        private readonly List<RegisterViewModel> lastChangedRegisters;

        public RegistersViewModel() : base("Registers", new ObservableCollection<RegisterViewModel>())
        {
            Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });

            Title = "Registers";
            lastChangedRegisters = new List<RegisterViewModel>();
        }

        public double ColumnWidth
        {
            get { return columnWidth; }
            set { this.RaiseAndSetIfChanged(ref columnWidth, value); }
        }

        public override Location DefaultLocation
        {
            get { return Location.Left; }
        }

        public void BeforeActivation()
        {
        }

        public void Activation()
        {
            _debugManager = IoC.Get<IDebugManager2>();

            IoC.Get<IStudio>().DebugPerspective.AddOrSelectTool(this);

            _debugManager.DebugSessionStarted += (sender, e) => { Enabled = false; };

            _debugManager.TargetReady += (sender, e) =>
            {
                if (_debugManager.ExtendedSession != null)
                {
                    IsVisible = true;

                    var regs = _debugManager.ExtendedSession.GetRegisters();

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        SetRegisters(regs);
                    });
                }
            };

            _debugManager.DebugSessionEnded += (sender, e) =>
            {
                IsVisible = false;
                Clear();
            };

            _debugManager.FrameChanged += (sender, e) =>
             {
                 Enabled = true;

                 if (_debugManager.ExtendedSession != null)
                 {

                     var changes = _debugManager.ExtendedSession.GetRegisterChanges();

                     UpdateRegisters(changes);
                 }
             };

            var started = Observable.FromEventPattern(_debugManager, nameof(_debugManager.TargetStarted));
            var stopped = Observable.FromEventPattern(_debugManager, nameof(_debugManager.TargetStopped));

            started.SelectMany(_ => Observable.Amb(Observable.Timer(TimeSpan.FromMilliseconds(250)).Select(o => true), stopped.Take(1).Select(o => false))).Where(timeout => timeout == true).Subscribe(s => Enabled = false);
        }

        private void SetRegisters(List<Register> registers)
        {
            if (registers != null)
            {
                Model = new ObservableCollection<RegisterViewModel>();

                foreach (var register in registers)
                {
                    Model.Add(new RegisterViewModel(register));
                }

                ColumnWidth = 0;
                ColumnWidth = double.NaN;
            }
        }

        public void Clear()
        {
            Model = new ObservableCollection<RegisterViewModel>();
            Enabled = false;
        }

        private void UpdateRegisters(Dictionary<int, string> updatedValues)
        {
            foreach (var register in lastChangedRegisters)
            {
                register.HasChanged = false;
            }

            lastChangedRegisters.Clear();

            foreach (var value in updatedValues)
            {
                var register = Model.FirstOrDefault(r => r.Index == value.Key);

                if (register != null)
                {
                    register.Value = value.Value;
                    register.HasChanged = true;

                    lastChangedRegisters.Add(register);
                }
            }

            ColumnWidth = 0;
            ColumnWidth = double.NaN;
        }

        public bool Enabled
        {
            get { return _enabled; }
            set { this.RaiseAndSetIfChanged(ref _enabled, value); }
        }
    }
}