﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WorkspaceServer.Packaging
{
    public class Package2 :
        IPackage,
        IHaveADirectory,
        IMightSupportBlazor,
        IHaveADirectoryAccessor
    {
        private readonly PackageDescriptor _descriptor;
        private readonly Dictionary<Type, PackageAsset> _assets = new Dictionary<Type, PackageAsset>();
        private IDirectoryAccessor directory;
        private bool _loaded;

        public Package2(
            string name,
            IDirectoryAccessor directoryAccessor) : this(new PackageDescriptor(name), directoryAccessor)
        {
        }

        public Package2(
            PackageDescriptor descriptor,
            IDirectoryAccessor directoryAccessor)
        {
            _descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));

            DirectoryAccessor = directoryAccessor ?? throw new ArgumentNullException(nameof(directoryAccessor));
        }

        public IEnumerable<PackageAsset> Assets => _assets.Values;

        public string Name => _descriptor.Name;

        public string Version => _descriptor.Version;

        public IDirectoryAccessor DirectoryAccessor { get; }

        public void Add(PackageAsset asset)
        {
            if (asset == null)
            {
                throw new ArgumentNullException(nameof(asset));
            }

            var packageRoot = DirectoryAccessor.GetFullyQualifiedRoot().FullName;
            var assetRoot = asset.DirectoryAccessor.GetFullyQualifiedRoot().FullName;

            if (!assetRoot.Contains(packageRoot))
            {
                throw new ArgumentException($"Asset must be located under package path: asset root ({assetRoot}) is not under package root ({packageRoot}).");
            }

            _assets.Add(asset.GetType(), asset);
        }

        public bool CanSupportBlazor => Assets.Any(a => a is WebAssemblyAsset);

        public DirectoryInfo Directory => DirectoryAccessor.GetFullyQualifiedRoot();

        IDirectoryAccessor IHaveADirectoryAccessor.Directory => DirectoryAccessor;

        public async Task EnsureLoadedAsync()
        {
            if (_loaded)
            {
                return;
            }

            foreach (var csproj in DirectoryAccessor.GetAllFilesRecursively()
                                                    .Where(f => f.Extension == ".csproj"))
            {
                Add(new ProjectAsset(DirectoryAccessor.GetDirectoryAccessorForRelativePath(csproj.Directory)));
            }

            _loaded = true;
        }
    }
}