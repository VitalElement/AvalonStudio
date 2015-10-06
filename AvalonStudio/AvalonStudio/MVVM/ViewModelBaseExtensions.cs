namespace AvalonStudio.MVVM
{
    using ReactiveUI;
    using System;
    using System.Linq.Expressions;
    using System.Runtime.CompilerServices;

    public static class ReactiveObjectExtensions
    {
        public static void OnPropertyChanged(this ReactiveObject reactiveObject, [CallerMemberName]string propertyName = null)
        {
            reactiveObject.RaisePropertyChanged(propertyName);
        }

        public static void RaisePropertyChanged<T>(this ReactiveObject reactiveObject, Expression<Func<T>> changedProperty)
        {
            string name = ((MemberExpression)changedProperty.Body).Member.Name;
            reactiveObject.OnPropertyChanged(name);
        }

        public static ReactiveObject Create(object model)
        {
            ReactiveObject result = null;

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
