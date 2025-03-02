using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using CnSharp.Updater.Server.Core;
using CnSharp.Updater.Server.V2.Controllers;
using NuGet.Server;
using NuGet.Server.Core.Infrastructure;

namespace CnSharp.Updater.Server.Controllers
{
    public class SharpUpdaterPackagesODataController : SharpUpdaterODataController
    {
        public SharpUpdaterPackagesODataController()
            : this(ServiceResolverHolder.Current)
        {
        }

        protected SharpUpdaterPackagesODataController(IServiceResolver serviceResolver)
            : base(serviceResolver.Resolve<IServerPackageRepository>(),
                   serviceResolver.Resolve<IPackageAuthenticationService>())
        {
            _maxPageSize = 100;
        }

        [HttpGet]
        // Exposed through ordinary Web API route. Bypasses OData pipeline.
        public async Task<HttpResponseMessage> ClearCache(CancellationToken token)
        {
            if (RequestContext.IsLocal || ServiceResolverHolder.Current.Resolve<ISettingsProvider>().GetBoolSetting("allowRemoteCacheManagement", false))
            {
                await _serverRepository.ClearCacheAsync(token);
                return CreateStringResponse(HttpStatusCode.OK, "Server cache has been cleared.");
            }
            else
            {
                return CreateStringResponse(HttpStatusCode.Forbidden, "Clear cache is only supported for local requests.");
            }
        }
    }
}