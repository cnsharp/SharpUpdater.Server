using NuGet.Server;

namespace CnSharp.Updater.Server.Core
{
    public class ServiceResolverHolder
    {
        private static readonly object SyncLock = new object();

        public static IServiceResolver Current { get; private set; }

        private static void EnsureServiceResolver()
        {
            if (Current == null)
            {
                lock (SyncLock)
                {
                    if (Current == null)
                    {
                        Current = new SharpUpdaterServiceResolver();
                    }
                }
            }
        }

        public static void SetServiceResolver(IServiceResolver serviceResolver)
        {
            Current = serviceResolver;
        }
    }
}