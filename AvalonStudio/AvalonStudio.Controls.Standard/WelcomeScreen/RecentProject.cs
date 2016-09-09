using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AvalonStudio.Utils;

namespace AvalonStudio.Controls.Standard.WelcomeScreen {
    public class RecentProject {
        public string Name { get; set; }
        public string Path { get; set; }
    }

    public static class RecentProjectsCollection {
        private static List<RecentProject> _recentProjects;
        public static List<RecentProject> RecentProjects {
            get {
                if (_recentProjects == null) {
                    _recentProjects = SerializedObject.Deserialize<List<RecentProject>>("./RecentProject.json");
                }

                return _recentProjects;
            }
            set { _recentProjects = value; }
        }

        public static void Save() {
            SerializedObject.Serialize("./RecentProject.json", RecentProjects);
        }
    }
}
