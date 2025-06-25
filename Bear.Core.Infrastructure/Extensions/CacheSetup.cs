using Bear.Core.Common.Extensions;
using Bear.Core.Core;
using Bear.Core.Core.Caches;
using Bear.Core.Core.Caches.Distributed;
using Bear.Core.Core.Caches.Redis;
using Bear.Core.Core.ConfigOptions;
using Microsoft.Extensions.DependencyInjection;

namespace Bear.Core.Infrastructure.Extensions;

/// <summary>
/// 缓存启动器
/// </summary>
public static class CacheSetup
{
    public static void AddCacheSetup(this IServiceCollection services)
    {
        if (services.IsNull())
            throw new ArgumentNullException(nameof(services));
        services.AddDistributedMemoryCache(); //session需要

        if (App.GetOptions<SystemOptions>().UseRedisCache)
        {
            services.AddSingleton<ICache, RedisCache>();
            return;
        }

        services.AddSingleton<ICache, DistributedCache>();
    }
}
