namespace AvalonStudio.Projects.Standard
{
    using System.Collections.Generic;

    public class StandardProjectFolder : IProjectFolder
    {
        public StandardProjectFolder(string name)
        {
            this.Name = name;
            Items = new List<IProjectItem>();
        }

        public IList<IProjectItem> Items { get; private set; }       

        public string Name { get; private set; }        
    }
}
