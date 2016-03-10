namespace AvalonStudio.Projects.Standard
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;

    public class StandardProjectFolder : IProjectFolder
    {
        public StandardProjectFolder(string path)
        {
            this.Name = System.IO.Path.GetFileName(path);
            this.Location = path;

            Items = new ObservableCollection<IProjectItem>();
        }

        public ObservableCollection<IProjectItem> Items { get; private set; }

        public string Name { get; private set; }

        public IProjectFolder Parent { get; set; }

        public string Location { get; private set; }

        public IProject Project { get; set; }

        public void AddFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public void AddFolder(IProjectFolder folder)
        {
            throw new NotImplementedException();
        }

        public void RemoveFile(ISourceFile file)
        {
            throw new NotImplementedException();
        }

        public void RemoveFolder(IProjectFolder folder)
        {
            throw new NotImplementedException();
        }
    }
}

