using NuGet;

namespace CnSharp.Updater.Server
{
    public static class ManifestExtensions
    {
        public static NuGet.Manifest ToNuGetManifest(this Manifest manifest)
        {
            return new NuGet.Manifest
            {
                Metadata = new ManifestMetadata
                {
                    Id = manifest.Id,
                    Description = manifest.Description,
                    Authors = manifest.Owner,
                    Owners = manifest.Owner,
                    ReleaseNotes = manifest.ReleaseNotes,
                    Version = manifest.Version,
                    IconUrl = manifest.IconUrl
                }
            };
        }
    }
}