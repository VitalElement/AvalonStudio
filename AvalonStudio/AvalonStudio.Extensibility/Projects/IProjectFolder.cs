namespace AvalonStudio.Projects
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public interface IProjectFolder : IProjectItem
    {
        /// <summary>
        /// List of items within the project folder
        /// </summary>
        ObservableCollection<IProjectItem> Items { get; }

        void AddFile(ISourceFile file);
        void AddFolder(IProjectFolder folder);

        void RemoveFile(ISourceFile file);
        void RemoveFolder(IProjectFolder folder);

        string Location { get; }
    }
}
