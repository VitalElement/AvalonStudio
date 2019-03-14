using AvalonStudio.MVVM;
using AvalonStudio.Packaging;

namespace AvalonStudio.Controls
{
    public class PackageIdentityViewModel : ViewModel<PackageIdentifier>
    {
        public PackageIdentityViewModel(PackageIdentifier model) : base(model)
        {
        }

        public string Title => Model.Name + " " + Version;

        public string Version => Model.Version.ToString();
    }
}