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

		public string Id { get; private set; }
		public string Value { get; private set; }
		public string Expression { get; private set; }
		public string Type { get; private set; }
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

		public static VariableObject FromDataString(VariableObject parent, string data, string expression = "")
		{
			var result = new VariableObject();

			result.Expression = expression;

			var pairs = data.ToNameValuePairs();

			foreach (var pair in pairs)
			{
				switch (pair.Name)
				{
					case "name":
						result.Id = pair.Value;
						break;

					case "numchild":
						result.NumChildren = Convert.ToInt32(pair.Value);
						break;

					case "value":
						result.Value = pair.Value;
						break;

					case "type":
						result.Type = pair.Value;
						break;

					case "thread-id":
						break;

					case "has_more":
						break;

					case "exp":
						result.Expression = pair.Value;
						break;

					default:
						Console.WriteLine("Unimplemented variable object field.");
						break;
				}
			}

			result.Parent = parent;

			return result;
		}
	}
}