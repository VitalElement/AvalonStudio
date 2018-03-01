namespace AvalonStudio.Projects
{
    class UnsupportedProjectType : PlaceHolderProject
    {
        /*public UnsupportedProjectType(string location)
        {
            Name = Path.GetFileName(location);
            Location = location;
            LocationDirectory = Path.GetDirectoryName(location);
            Project = this;
            Parent = this;
        }*/

        public UnsupportedProjectType(ISolution solution, string location) : base(solution, location)
        {
        }
    }
}
