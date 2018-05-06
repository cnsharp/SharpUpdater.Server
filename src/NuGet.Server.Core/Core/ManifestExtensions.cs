namespace NuGet.Server.Core
{
    public static class ManifestExtensions
    {
        public static Manifest ToNuGetManifest(this CnSharp.Updater.Manifest manifest)
        {
            return new Manifest
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