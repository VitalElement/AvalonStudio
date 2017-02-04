using System.Composition;
using System.Linq;
using AvalonStudio.Extensibility.Commands;
using AvalonStudio.Extensibility.ToolBars.Models;

namespace AvalonStudio.Extensibility.ToolBars
{
	[Export(typeof (IToolBarBuilder))]
	public class ToolBarBuilder : IToolBarBuilder
	{
		private readonly ToolBarItemGroupDefinition[] _toolBarItemGroups;
		private readonly ToolBarItemDefinition[] _toolBarItems;
		private readonly ToolBarDefinition[] _toolBars;

		[ImportingConstructor]
		public ToolBarBuilder(	
			[ImportMany] ToolBarDefinition[] toolBars,
			[ImportMany] ToolBarItemGroupDefinition[] toolBarItemGroups,
			[ImportMany] ToolBarItemDefinition[] toolBarItems,
			[ImportMany] ExcludeToolBarDefinition[] excludeToolBars,
			[ImportMany] ExcludeToolBarItemGroupDefinition[] excludeToolBarItemGroups,
			[ImportMany] ExcludeToolBarItemDefinition[] excludeToolBarItems)
		{			
			_toolBars = toolBars
				.Where(x => !excludeToolBars.Select(y => y.ToolBarDefinitionToExclude).Contains(x))
				.ToArray();
			_toolBarItemGroups = toolBarItemGroups
				.Where(x => !excludeToolBarItemGroups.Select(y => y.ToolBarItemGroupDefinitionToExclude).Contains(x))
				.ToArray();
			_toolBarItems = toolBarItems
				.Where(x => !excludeToolBarItems.Select(y => y.ToolBarItemDefinitionToExclude).Contains(x))
				.ToArray();
		}

		public void BuildToolBars(IToolBars result)
		{
			var toolBars = _toolBars.OrderBy(x => x.SortOrder);

			foreach (var toolBar in toolBars)
			{
				var toolBarModel = new ToolBarModel();
				BuildToolBar(toolBar, toolBarModel);
				if (toolBarModel.Any())
					result.Items.Add(toolBarModel);
			}
		}

		public void BuildToolBar(ToolBarDefinition toolBarDefinition, IToolBar result)
		{
			var groups = _toolBarItemGroups
				.Where(x => x.ToolBar == toolBarDefinition)
				.OrderByDescending(x => x.SortOrder)
				.ToList();

			for (var i = 0; i < groups.Count; i++)
			{
				var group = groups[i];
				var toolBarItems = _toolBarItems
					.Where(x => x.Group == group)
					.OrderByDescending(x => x.SortOrder);

				/*foreach (var toolBarItem in toolBarItems)
					result.Add(new CommandToolBarItem(toolBarItem, _commandService.GetCommand(toolBarItem.CommandDefinition),
						toolBarItem.CommandDefinition.Command, result));*/

				if (i < groups.Count - 1 && toolBarItems.Any())
					result.Add(new ToolBarItemSeparator());
			}
		}
	}
}