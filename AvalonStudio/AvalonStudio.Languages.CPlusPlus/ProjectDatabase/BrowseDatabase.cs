namespace AvalonStudio.Languages.CPlusPlus.ProjectDatabase
{
    using AvalonStudio.Projects;
    using AvalonStudio.Utils;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    public class BrowseDatabase
    {
        private ProjectContext db;

        public BrowseDatabase(ISolution solution)
        {
            db = new ProjectContext(solution);

            db.Database.Migrate();
        }
        
        public SourceFile GetOrCreateSourceFile(ISourceFile file, out bool modified)
        {
            modified = false;

            var existingFile = db.SourceFiles.FirstOrDefault(f => f.RelativePath == file.Project.Location.MakeRelativePath(file.Location));

            var lastModified = System.IO.File.GetLastWriteTimeUtc(file.Location);            

            if (existingFile == null)
            {
                modified = true;
                existingFile = new SourceFile() { RelativePath = file.Project.Location.MakeRelativePath(file.Location), LastModified = lastModified };
                db.SourceFiles.Add(existingFile);
            }
            else if (lastModified > existingFile.LastModified)
            {
                modified = true;
            }

            return existingFile;
        }

        public SymbolReference GetSymbolReference(string reference)
        {            
            return db.UniqueReferences.Include(sr=>sr.Symbols).FirstOrDefault(r => r.Reference == reference);
        }

        public void AddSymbolReferences(IEnumerable<SymbolReference> references)
        {
            db.UniqueReferences.AddRange(references);
        }

        public void AddSymbols (IEnumerable<Symbol> symbols)
        {
            db.Symbols.AddRange(symbols);
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }
    }
}
