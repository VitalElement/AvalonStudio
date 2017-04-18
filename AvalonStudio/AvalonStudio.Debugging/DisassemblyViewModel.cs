using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Threading;
using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Plugin;
using AvalonStudio.MVVM;
using ReactiveUI;
using AvalonStudio.MVVM.DataVirtualization;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging
{
	public abstract class LineViewModel : ViewModel<DisassembledLine>
	{
		public LineViewModel(DisassembledLine model) : base(model)
		{
		}
	}

	public class InstructionLineViewModel : LineViewModel
	{
		public InstructionLineViewModel(InstructionLine model) : base(model)
		{
		}

		public string Instruction
		{
			get { return Model.Instruction; }
		}

		public string Address
		{
			get
			{
				try
				{
					return string.Format("0x{0:X}", Model.Address);
				}
				catch (Exception)
				{
					return "";
				}
			}
		}

		public string Symbol
		{
			get { return string.Format("<{0} + {1}>", Model.FunctionName, Model.Offset); }
		}

		public new InstructionLine Model
		{
			get { return (InstructionLine) base.Model; }
		}
	}

	public class SourceLineViewModel : LineViewModel
	{
		public SourceLineViewModel(SourceLine model) : base(model)
		{
		}

		public string LineText
		{
			get { return Model.LineText; }
		}

		public new SourceLine Model
		{
			get { return (SourceLine) base.Model; }
		}
	}


	public class DisassemblyViewModel : ToolViewModel, IExtension
	{
		private IDebugger2 _debugger;
		private IDebugManager2 _debugManager;

		private readonly DisassemblyDataProvider dataProvider;

		private AsyncVirtualizingCollection<InstructionLine> disassemblyData;

		private bool enabled;

		private ulong selectedIndex;

		public DisassemblyViewModel()
		{
			Dispatcher.UIThread.InvokeAsync(() => { IsVisible = false; });

			Title = "Disassembly";
			dataProvider = new DisassemblyDataProvider();
		}

		public bool Enabled
		{
			get { return enabled; }
			set { this.RaiseAndSetIfChanged(ref enabled, value); }
		}

		public ulong SelectedIndex
		{
			get { return selectedIndex; }
			set { this.RaiseAndSetIfChanged(ref selectedIndex, value); }
		}

		public AsyncVirtualizingCollection<InstructionLine> DisassemblyData
		{
			get { return disassemblyData; }
			set { this.RaiseAndSetIfChanged(ref disassemblyData, value); }
		}

		public override Location DefaultLocation
		{
			get { return Location.RightTop; }
		}

		public void Activation()
		{
			_debugManager = IoC.Get<IDebugManager2>();
			//_debugManager.DebuggerChanged += (sender, e) => { SetDebugger(_debugManager.CurrentDebugger); };

			//_debugManager.DebugFrameChanged += _debugManager_DebugFrameChanged;

			_debugManager.DebugSessionStarted += (sender, e) => { IsVisible = true; };

			_debugManager.DebugSessionEnded += (sender, e) =>
			{
				IsVisible = false;

                // TODO clear out data ready for GC, this requires a fix in Avalonia.
                //DisassemblyData = null;
			};
		}

		public void BeforeActivation()
		{
		}
        
		public void SetAddress(ulong currentAddress)
		{
            if (DisassemblyData == null)
            {
                DisassemblyData = new AsyncVirtualizingCollection<InstructionLine>(dataProvider, 100, 6000);

                Task.Factory.StartNew(async () =>
                {
                    await Task.Delay(50);

                    Dispatcher.UIThread.InvokeAsync(() =>
                    {
                        SelectedIndex = currentAddress;
                    });
                });
            }
            else
            {
                SelectedIndex = currentAddress;
            }
		}
	}
}