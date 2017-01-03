using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    public class ProjectContext : DbContext
    {
        public DbSet<SourceFiles> SourceFiles { get; set; }
    }
}
