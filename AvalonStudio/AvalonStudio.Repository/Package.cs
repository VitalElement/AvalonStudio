namespace AvalonStudio.Repositories
{
    using AvalonStudio.Utils;

    public class Package : SerializedObject<Package>
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
