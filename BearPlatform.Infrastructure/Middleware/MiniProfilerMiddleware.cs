using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper.Serilog;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using Microsoft.AspNetCore.Builder;
using Serilog;

namespace BearPlatform.Infrastructure.Middleware;

/// <summary>
/// 性能监控中间件
/// </summary>
public static class MiniProfilerMiddleware
{
    private static readonly ILogger Logger = SerilogManager.GetLogger(typeof(MiniProfilerMiddleware));

    public static void UseMiniProfilerMiddleware(this IApplicationBuilder app)
    {
        if (app.IsNull())
            throw new ArgumentNullException(nameof(app));
        try
        {
            if (App.GetOptions<MiddlewareOptions>().MiniProfiler.Enabled)
            {
                // 性能分析
                app.UseMiniProfiler();
            }
        }
        catch (Exception e)
        {
            Logger.Error($"Performance monitoring startup error:\n{e.Message}");
            throw;
        }
    }
}
