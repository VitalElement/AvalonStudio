using Perspex.MVVM;

namespace VEStudio.MVVM
{
    public static class ViewModelBaseExtensions
    {
        public static ViewModelBase Create(object model)
        {
            ViewModelBase result = null;

            //if (model is ProjectFile)
            //{
            //    result = new ProjectFileViewModel(model as ProjectFile);
            //}
            //else if (model is BitThunderApplicationProject)
            //{
            //    result = new BitThunderProjectViewModel(model as BitThunderApplicationProject);
            //}
            //else if (model is CatchTestProject)
            //{
            //    result = new TestProjectViewModel(model as CatchTestProject);
            //}
            //else if (model is Project)
            //{
            //    result = new StandardProjectViewModel(model as Project);
            //}
            //else if (model is ProjectFolder)
            //{
            //    result = new ProjectFolderViewModel(model as ProjectFolder);
            //}
            //else if (model is SolutionFolder)
            //{
            //    result = new SolutionFolderViewModel(model as SolutionFolder);
            //}

            return result;
        }
    }
}
