namespace AvalonStudio.Repositories
{
    using AvalonStudio.Utils;

    public class Package : SerializedObject<Package>
    {
        public Repository Repository { get; internal set; }
        public const string PackageFile = "package.json";
        public string Plugin { get; set; }
    }
}
