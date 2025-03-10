using System;
using System.Diagnostics;
using System.Text;
using CnSharp.Updater.Server.Packages;
using NuGet;

namespace CnSharp.Updater.Server
{
    /// <summary>
    /// Copy from https://github.com/NuGet/NuGet2/blob/main/src/Core/Packages/ZipPackageAssemblyReference.cs
    /// </summary>
    internal class ZipPackageAssemblyReference : ZipPackageFile, IPackageAssemblyReference
    {
        public ZipPackageAssemblyReference(IPackageFile file)
            : base(file)
        {
            Debug.Assert(Path.StartsWith("lib", StringComparison.OrdinalIgnoreCase), "path doesn't start with lib");
        }

        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(Path);
            }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            if (TargetFramework != null)
            {
                builder.Append(TargetFramework).Append(" ");
            }
            builder.Append(Name).AppendFormat(" ({0})", Path);
            return builder.ToString();
        }
    }
}