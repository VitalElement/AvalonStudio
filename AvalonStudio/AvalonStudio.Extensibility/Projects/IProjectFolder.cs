namespace AvalonStudio.Projects
{
    using System.Collections.Generic;

    public interface IProjectFolder : IProjectItem
    {
        /// <summary>
        /// List of items within the project folder
        /// </summary>
        IList<IProjectItem> Items { get; }
    }
}
