// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 
using System;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using NuGet.Resources;

namespace NuGet.Server.Core
{
    public static class PackageExtensions
    {
        public static bool IsSymbolsPackage(this IPackage package)
        {
            var hasSymbols = package.GetFiles()
                .Any(pf => string.Equals(Path.GetExtension(pf.Path), ".pdb", StringComparison.InvariantCultureIgnoreCase));

            return hasSymbols && package.GetFiles()
                   .Any(pf => pf.Path.StartsWith("src") || pf.Path.StartsWith("/src"));
        }

        public static Stream GetManifestStream(this Stream packageStream)
        {
            Package package = Package.Open(packageStream);

            return GetManifestStream(package);
        }

        public static Stream GetManifestStream(this Package package)
        {

            PackageRelationship relationshipType = package.GetRelationshipsByType(CnSharp.Updater.Packaging.PackageBuilder.PackageRelationshipNamespace + CnSharp.Updater.Packaging.PackageBuilder.ManifestRelationType).SingleOrDefault();

            if (relationshipType == null)
            {
                throw new InvalidOperationException(NuGetResources.PackageDoesNotContainManifest);
            }

            PackagePart manifestPart = package.GetPart(relationshipType.TargetUri);

            if (manifestPart == null)
            {
                throw new InvalidOperationException(NuGetResources.PackageDoesNotContainManifest);
            }

            return manifestPart.GetStream();
        }
    }
}