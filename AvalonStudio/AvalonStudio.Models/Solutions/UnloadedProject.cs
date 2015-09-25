namespace AvalonStudio.Models.Solutions
{
    using System.IO;

    public class UnloadedProject : Item
    {
        private UnloadedProject() : base (null)
        {

        }

        public UnloadedProject (Item parent, string fileName) : base (parent)
        {
            FileName = fileName;
            this.Name = Path.GetFileNameWithoutExtension(fileName);
        }

        public UnloadedProject(Item parent) : base(parent)
        {
        }

        public string Name { get; set; }

        public override string FileName { get; set; }
    }
}
