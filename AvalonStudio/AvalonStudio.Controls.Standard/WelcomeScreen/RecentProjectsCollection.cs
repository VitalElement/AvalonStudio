namespace AvalonStudio.Controls.Standard.WelcomeScreen
{
    using AvalonStudio.Platforms;
    using AvalonStudio.Utils;
    using System;
    using System.Collections.Generic;
    using System.IO;
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
            set
            {
                _recentProjects = value;
            }
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