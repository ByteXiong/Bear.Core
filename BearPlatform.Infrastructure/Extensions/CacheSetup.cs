using BearPlatform.Common.Extensions;
using BearPlatform.Core;
using BearPlatform.Core.Caches;
using BearPlatform.Core.Caches.Distributed;
using BearPlatform.Core.Caches.Redis;
using BearPlatform.Core.ConfigOptions;
using Microsoft.Extensions.DependencyInjection;

namespace BearPlatform.Infrastructure.Extensions;

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
