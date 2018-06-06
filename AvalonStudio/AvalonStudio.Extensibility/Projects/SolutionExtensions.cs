using AvalonStudio.Extensibility;
using AvalonStudio.Extensibility.Studio;
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
            return IoC.Get<IStudio>().ProjectTypes.FirstOrDefault(
                type => type.Metadata.ProjectTypeGuid == projectTypeId)?.Value;
        }

        internal static void SetParentInternal(this ISolutionItem item, ISolutionFolder parent)
        {
            if (item.Parent != null)
            {
                item.Parent.Items.Remove(item);
            }

            item.Parent = parent;
            
            parent?.Items.InsertSorted(item);
        }

        internal static string GetGuidString(this Guid id)
        {
            return id.ToString("B").ToUpper();
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

        public static int DefaultCompareTo(this ISolutionItem item, ISolutionItem other)
        {
            if (item is ISolutionFolder && other is IProject)
            {
                return -1;
            }
            else if (item is IProject && other is ISolutionFolder)
            {
                return 1;
            }
            else if(item is IProject && other is IProject)
            {
                return (item as IProject).CompareTo(other as IProject);
            }
            else
            {
                return item.Name.CompareTo(other.Name);
            }
        }
    }
}
