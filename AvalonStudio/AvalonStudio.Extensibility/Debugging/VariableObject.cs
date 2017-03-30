using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Debugging
{
	public enum VariableObjectType
	{
		Fixed,
		Floating
	}

	public class VariableObject
	{
		private IDebugger debugger;
		private WatchFormat format;

		public VariableObject()
		{
			Children = new List<VariableObject>();
			format = WatchFormat.Natural;
		}

		public VariableObject Parent { get; set; }

		public int NumChildren { get; set; }

		public string Id { get; set; }
		public string Value { get; set; }
		public string Expression { get; set; }
		public string Type { get; set; }
		public List<VariableObject> Children { get; }

		public bool AreChildrenEvaluated { get; private set; }


		public async Task EvaluateChildrenAsync()
		{
			if (!AreChildrenEvaluated)
			{
				await EvaluateChildrenInternalAsync();
				AreChildrenEvaluated = true;
			}
		}

		public void ClearEvaluated()
		{
			AreChildrenEvaluated = false;
		}

		private async Task EvaluateChildrenInternalAsync()
		{
			Children.Clear();

			var newChildren = await debugger.ListChildrenAsync(this);

			if (newChildren != null)
			{
				Children.AddRange(newChildren);
			}

			foreach (var child in Children)
			{
				await child.EvaluateAsync(debugger, false);
			}
		}


		public async Task EvaluateAsync(IDebugger debugger, bool evaluateChildren = true)
		{
			this.debugger = debugger;

			Value = await debugger.EvaluateExpressionAsync(Id);

			if (NumChildren > 0 && evaluateChildren)
			{
				await EvaluateChildrenInternalAsync();
			}
		}

		public async Task SetFormat(WatchFormat format)
		{
			if (this.format != format)
			{
				await debugger.SetWatchFormatAsync(Id, format);
				this.format = format;
			}
		}
	}
}