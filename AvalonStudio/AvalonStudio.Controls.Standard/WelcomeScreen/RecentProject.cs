namespace AvalonStudio.Controls.Standard.WelcomeScreen
{
    using System;

    public class RecentProject : IEquatable<RecentProject>
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public bool Equals(RecentProject other)
        {
            return Name == other.Name && Path == other.Path;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RecentProject);
        }

        public override int GetHashCode() => (string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode()) + (string.IsNullOrEmpty(Path) ? 0 : Path.GetHashCode());        
    }
}