namespace AvalonStudio.Controls.Standard.WelcomeScreen
{
    using System;

    public class RecentProject : IEquatable<RecentProject>
    {
        public string Name { get; set; }
        public string Path { get; set; }

        public bool Equals(RecentProject other)
        {
            return (Name == other.Name && Path == other.Path);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as RecentProject);
        }
    }
}