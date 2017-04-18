using AvalonStudio.MVVM;
using NuGet.Protocol.Core.Types;

namespace AvalonStudio.Controls
{
    public class VersionInfoViewModel : ViewModel<VersionInfo>
    {
        public VersionInfoViewModel(VersionInfo model) : base(model)
        {
        }

        public string Title => Model.Version.ToNormalizedString();
    }
}