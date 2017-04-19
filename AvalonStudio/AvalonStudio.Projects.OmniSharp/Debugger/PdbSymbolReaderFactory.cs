namespace AvalonStudio.Debugging.DotNetCore
{
    using Mono.Debugging.Win32;
    using System.Diagnostics.SymbolStore;
    using System.IO;

    public class PdbSymbolReaderFactory : ICustomCorSymbolReaderFactory
    {
        private bool IsPortablePdbFormat(string fileName)
        {
            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // Read first 4bytes and check if it matched portable pdb files.
                return (int)new BinaryReader(fileStream).ReadUInt32() == 1112167234;
            }
        }

        public ISymbolReader CreateCustomSymbolReader(string assemblyLocation)
        {
            if (!System.IO.File.Exists(assemblyLocation))
            {
                return null;
            }

            string pdbLocation = System.IO.Path.ChangeExtension(assemblyLocation, "pdb");

            if (!System.IO.File.Exists(pdbLocation))
            {
                return null;
            }

            if (!IsPortablePdbFormat(pdbLocation))
            {
                return null;
            }

            return new PdbSymbolReader(pdbLocation);
        }
    }
}