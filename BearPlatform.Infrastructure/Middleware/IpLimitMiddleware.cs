using AspNetCoreRateLimit;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper.Serilog;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace BearPlatform.Infrastructure.Middleware;

/// <summary>
/// IP限流策略中间件
/// </summary>
public static class IpLimitMiddleware
{
    private static readonly ILogger Logger = SerilogManager.GetLogger(typeof(IpLimitMiddleware));

    public static void UseIpLimitMiddleware(this IApplicationBuilder app)
    {
        if (app.IsNull())
            throw new ArgumentNullException(nameof(app));
        try
        {
            if (App.GetOptions<MiddlewareOptions>().IpLimit.Enabled)
            {
                app.UseIpRateLimiting();
            }
        }
        catch (Exception e)
        {
            Logger.Error($"Error occured limiting ip rate.\n{e.Message}");
            throw;
        }
    }
}
