using Bear.Core.Common.Extensions;
using Bear.Core.Common.Helper;
using Bear.Core.Common.Helper.Serilog;
using Bear.Core.Core;
using Bear.Core.Core.ConfigOptions;
using Bear.Core.IBusiness.System;
using Bear.Core.TaskService.service;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Bear.Core.Infrastructure.Middleware;

/// <summary>
/// QuartzNet作业调度中间件
/// </summary>
public static class QuartzNetJobMiddleware
{
    private static readonly ILogger Logger = SerilogManager.GetLogger(typeof(QuartzNetJobMiddleware));

    public static void UseQuartzNetJobMiddleware(this IApplicationBuilder app)
    {
        if (app.IsNull())
            throw new ArgumentNullException(nameof(app));

        try
        {
            if (App.GetOptions<MiddlewareOptions>().QuartzNetJob.Enabled)
            {
                var quartzNetService = app.ApplicationServices.GetRequiredService<IQuartzNetService>();
                var schedulerCenter = app.ApplicationServices.GetRequiredService<ISchedulerCenterService>();
                var allTaskQuartzList = AsyncHelper.RunSync(() => quartzNetService.QueryAllAsync());
                foreach (var item in allTaskQuartzList)
                {
                    if (!item.IsEnable) continue;
                    var results = AsyncHelper.RunSync(() => schedulerCenter.AddScheduleJobAsync(item));
                    Logger.Information(results
                        ? $"{App.L.R("Sys.QuartzNet")}=>{item.TaskName}=>{App.L.R("Action.StartupSSuccess")}！"
                        : $"{App.L.R("Sys.QuartzNet")}=>{item.TaskName}=>{App.L.R("Action.StartupFailure")}！");
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error($"Error starting the job scheduling service:\n{e.Message}");
            throw;
        }
    }
}
