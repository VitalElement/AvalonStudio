namespace AvalonStudio.Repositories
{
    using System.Collections.Generic;
    using AvalonStudio.Utils;

    public class PackageReference
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Repository : SerializedObject<Repository>
    {
        public const string PackagesFileName = "Packages.json";
         
        public Repository()
        {
            Packages = new List<PackageReference>();
        }

        public IList<PackageReference> Packages { get; set; }
    }
}
