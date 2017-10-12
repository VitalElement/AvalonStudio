using AvalonStudio.Extensibility;
using AvalonStudio.Shell;
using AvalonStudio.Utils;
using Microsoft.DotNet.Cli.Sln.Internal;
using System;
using System.Linq;

namespace AvalonStudio.Projects
{
    public static class SolutionExtensions
    {
        public static IProjectType GetProjectType(this SlnProject solutionModel)
        {
            return Guid.Parse(solutionModel.TypeGuid).GetProjectType();
        }

        public static IProjectType GetProjectType(this Guid projectTypeId)
        {
            return IoC.Get<IShell>().ProjectTypes.FirstOrDefault(type => type.ProjectTypeId == projectTypeId);
        }

        public static void SetParent(this ISolutionItem item, ISolutionFolder parent)
        {
            if (item.Parent != null)
            {
                item.Parent.Items.Remove(item);
            }

            item.Parent = parent ?? throw new ArgumentNullException("parent");
            
            parent.Items.InsertSorted(item);
        }

        public static int GetSolutionItemDepth (this ISolutionItem item)
        {
            int depth = 0;

            var topLevel = item.Solution;
            var currentItem = item;

            while(currentItem != topLevel)
            {
                currentItem = currentItem.Parent;
                depth++;
            }

            return depth;
        }

        public static void PrintTree (this ISolutionItem item)
        {
            var depth = item.GetSolutionItemDepth();

            var indentation = new string('-', depth * 2) + ">";

            Console.WriteLine(indentation + item.Name);

            if(item is ISolutionFolder folder)
            {
                foreach (var child in folder.Items)
                {
                    child.PrintTree();
                }
            }
        }
    }
}
