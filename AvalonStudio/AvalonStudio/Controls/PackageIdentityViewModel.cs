using AvalonStudio.MVVM;
using NuGet.Packaging.Core;

namespace AvalonStudio.Controls
{
    public class PackageIdentityViewModel : ViewModel<PackageIdentity>
    {
        public PackageIdentityViewModel(PackageIdentity model) : base(model)
        {
        }

        public string Title => Model.Id;
    }
}