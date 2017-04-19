using System.Collections.Generic;

namespace AvalonStudio.Repositories
{
    public class RepositoryOld
    {
        public const string PackagesFileName = "packages.json";

        public RepositoryOld()
        {
            Packages = new List<PackageReference>();
        }

        public PackageSourceOld Source { get; internal set; }
        public IList<PackageReference> Packages { get; set; }
    }
}