// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information. 

namespace CnSharp.Updater.Server
{
    public static class Constants
    {
        public const string HashAlgorithm = "SHA512";
        public const string ProductName = "SharpUpdater";

        internal const string PackageRelationshipNamespace = "http://schemas.microsoft.com/packaging/2010/07/";

        internal const string ManifestRelationType = "manifest";

        public const string UriId = "sp";

        /// <summary>
        /// Represents the ".sp" extension.
        /// </summary>
        public static readonly string PackageExtension = ".sp";

        /// <summary>
        /// Represents the ".sp.sha512" extension.
        /// </summary>
        public static readonly string HashFileExtension = PackageExtension + ".sha512";
    }
}