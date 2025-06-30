using AspNetCoreRateLimit;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using Microsoft.Extensions.DependencyInjection;

namespace BearPlatform.Infrastructure.Extensions;

/// <summary>
/// IP限流启动器
/// </summary>
public static class IpRateLimitSetup
{
    public static void AddIpStrategyRateLimitSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        services.Configure<IpRateLimitOptions>(App.Configuration.GetSection("IpRateLimiting"));
        services.Configure<IpRateLimitPolicies>(App.Configuration.GetSection("IpRateLimitPolicies"));

        if (App.GetOptions<SystemOptions>().UseRedisCache)
        {
            var redisOptions = App.GetOptions<RedisOptions>();
            services.AddStackExchangeRedisCache(option =>
            {
                if (!string.IsNullOrWhiteSpace(redisOptions.Password))
                {
                    option.Configuration =
                        $"{redisOptions.Host}:{redisOptions.Port},password={redisOptions.Password},defaultDatabase={redisOptions.Index + 2}";
                }
                else
                {
                    option.Configuration = redisOptions.Host + ":" + redisOptions.Port;
                }

                option.InstanceName = "rateLimit:";
            });
            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
        }
        else
        {
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        }

        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
    }
}
