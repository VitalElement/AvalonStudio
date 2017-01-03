namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    using AvalonStudio.Projects.CPlusPlus;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProjectContext : DbContext
    {
        private string _fileName;

        public ProjectContext(CPlusPlusProject project)
        {
            var directory = $"{project.Solution.CurrentDirectory}.AvalonStudio";

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _fileName = $"{directory}\\Browse.db";
        }

        public DbSet<SourceFiles> SourceFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_fileName}");
        }
    }
}
