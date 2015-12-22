namespace AvalonStudio.Projects.Standard
{
    using System.Collections.Generic;

    public class StandardProjectFolder : IProjectFolder
    {
        public StandardProjectFolder(string path)
        {
            this.Name = System.IO.Path.GetFileName(path);
            this.Path = path;

            Items = new List<IProjectItem>();
        }

        public IList<IProjectItem> Items { get; private set; }       

        public string Name { get; private set; }        

        public string Path { get; private set; }
    }
}
