namespace AvalonStudio.Controls.Standard.WelcomeScreen
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using AvalonStudio.Platforms;
    using AvalonStudio.Utils;

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

    public static class RecentProjectsCollection
    {
        private static string _savePath = Path.Combine(Platform.SettingsDirectory, "RecentProject.json");

        private static List<RecentProject> _recentProjects;

        public static List<RecentProject> RecentProjects
        {
            get
            {
                if (_recentProjects == null)
                {
                    Deserialize();
                }

                return _recentProjects;
            }
            set { _recentProjects = value; }
        }

        public static void Save()
        {
            try
            {
                SerializedObject.Serialize(_savePath, RecentProjects);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void Deserialize()
        {
            try
            {
                _recentProjects = SerializedObject.Deserialize<List<RecentProject>>(_savePath);
            }
            catch (Exception)
            {
                _recentProjects = new List<RecentProject>();
            }
        }

    }
}
