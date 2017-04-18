using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using ReactiveUI;

namespace AvalonStudio.Debugging
{
	public class RegistersViewModel : ToolViewModel<ObservableCollection<RegisterViewModel>>, IExtension
	{
		private IDebugManager2 _debugManager;

		private double columnWidth;

		private bool firstStopInSession;

		private readonly List<RegisterViewModel> lastChangedRegisters;

		public RegistersViewModel() : base(new ObservableCollection<RegisterViewModel>())
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

			_debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

			_debugManager.DebugSessionEnded += (sender, e) =>
			{
				IsVisible = false;
				Clear();
			};
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

		public new async void Invalidate()
		{
			if (firstStopInSession)
			{
				firstStopInSession = false;

				List<Register> registers = null;

				SetRegisters(registers);
			}
			else
			{
				Dictionary<int, string> changedRegisters = null;

				Dispatcher.UIThread.InvokeAsync(() => { UpdateRegisters(changedRegisters); });
			}
		}

		public void Clear()
		{
			Model = new ObservableCollection<RegisterViewModel>();
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
	}
}