namespace AvalonStudio.Models.Solutions
{
    using System;
    using System.Xml.Serialization;

    [Serializable]
    [XmlInclude(typeof(UnloadedProject))]
    [XmlInclude(typeof(SolutionFolder))]
    public abstract class Item
    {
        private Item ()
        {
            Id = Guid.NewGuid();
            UserData = new ProjectItemUserData(Id);
        }

        public Item(Item parent) : this()
        {
            if(parent != null)
            {
                ParentId = parent.Id;
            }
        }
        
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        [XmlIgnore]
        public abstract string FileName { get; set; }

        [XmlIgnore]
        public virtual ProjectItemUserData UserData { get; set; }        
    }
}
