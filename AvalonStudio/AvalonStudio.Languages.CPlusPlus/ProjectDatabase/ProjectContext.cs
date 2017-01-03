namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ProjectContext : DbContext
    {
        public DbSet<SourceFiles> SourceFiles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=./contact.db");
        }
    }
}
