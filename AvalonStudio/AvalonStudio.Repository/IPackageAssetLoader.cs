﻿using AvalonStudio.Extensibility.Plugin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AvalonStudio.Packages
{
    public interface IPackageAssetLoader : IActivatableExtension
    {
        Task LoadAssetsAsync(string package, string version, IEnumerable<string> files);
    }
}
