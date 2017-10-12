using AvalonStudio.Projects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Microsoft.DotNet.Cli.Sln.Internal;
using System.IO;

namespace AvalonStudio.Extensibility.Projects
{
    public class VisualStudioSolution : ISolution
    {
        private SlnFile _solutionModel;

        public static VisualStudioSolution Load(string fileName)
        {
            var result = new VisualStudioSolution(SlnFile.Read(fileName));



            return result;
        }

        private VisualStudioSolution(SlnFile solutionModel)
        {
            _solutionModel = solutionModel;
        }

        public string Name => Path.GetFileNameWithoutExtension(_solutionModel.FullPath);

        public string Location => _solutionModel.FullPath;

        public IProject StartupProject { get; set; }

        public ObservableCollection<IProject> Projects { get; set; }

        public string CurrentDirectory => Path.GetDirectoryName(_solutionModel.FullPath);

        public ObservableCollection<ISolutionItem> Items => null;

        public IProject AddProject(IProject project)
        {
            throw new NotImplementedException();
        }

        public ISourceFile FindFile(string path)
        {
            throw new NotImplementedException();
        }

        public void RemoveProject(IProject project)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _solutionModel.Write();
        }
    }
}
