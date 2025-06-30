using BearPlatform.Common.Extensions;
using BearPlatform.Core;
using IP2Region.Net.Abstractions;
using IP2Region.Net.XDB;
using Microsoft.Extensions.DependencyInjection;

namespace BearPlatform.Infrastructure.Extensions
{
    public static class IpSearcherSetup
    {
        public static void AddIpSearcherSetup(this IServiceCollection services)
        {
            if (services.IsNull()) throw new ArgumentNullException(nameof(services));
            services.AddSingleton<ISearcher>(new Searcher(CachePolicy.Content,
                Path.Combine(App.WebHostEnvironment.WebRootPath, "resources", "ip", "ip2region.xdb")));
        }
    }
}
