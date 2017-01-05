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

        public ProjectContext()
        {

        }

        public DbSet<SourceFiles> SourceFiles { get; set; }
        public DbSet<Symbol> Symbols { get; set; }        
        public DbSet<SymbolReference> UniqueReferences { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SymbolReference>()
            .HasIndex(b => b.Reference)
            .IsUnique();

            //modelBuilder.Entity<SymbolReference>().HasOne(sr => sr.Definition).WithOne().HasForeignKey<SymbolReference>(sr => sr.DefinitionForeignKey);
            //modelBuilder.Entity<Symbol>().HasOne(s => s.USR).WithOne().HasForeignKey<Symbol>(s=>s.USRForeignKey);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={_fileName}");
        }
    }
}
