namespace AvalonStudio.Models.Solutions
{
    using System.IO;

    public class BitThunderApplicationProject : Project
    {
        private BitThunderApplicationProject() : base ()
        {
            
        }

        public BitThunderApplicationProject(Solution solution, Item container)
            : base(solution, container)
        {
            
        }        

        new public static BitThunderApplicationProject Create(Solution solution, SolutionFolder container, string name)
        {
            var result = new BitThunderApplicationProject(solution, container);

            string newFolder = Path.Combine(solution.CurrentDirectory, name);

            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }

            result.LocationRelativeToParent = Path.Combine(name, name + VEStudioService.ProjectExtension);

            result.Configurations.Add(new ProjectConfiguration() { Name = "Default" });

            result.SerializeToXml();

            container.AddProject(result, result.GetUnloadedProject());

            return result;
        }
    }
}
