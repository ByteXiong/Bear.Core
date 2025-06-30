using BearPlatform.Common.Enums;
using BearPlatform.Common.Helper.Serilog;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.Core.SeedData;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace BearPlatform.Infrastructure.Middleware;

public static class DataSeederMiddleware
{
    private static readonly ILogger Logger = SerilogManager.GetLogger(typeof(DataSeederMiddleware));

    public static void UseDataSeederMiddleware(this IApplicationBuilder app)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));

        try
        {
            var systemOptions = App.GetOptions<SystemOptions>();
            var tenantOptions = App.GetOptions<TenantOptions>();
            if (systemOptions.IsInitTable)
            {
                var dataContext = app.ApplicationServices.GetRequiredService<DataContext>();
                SeedService.InitMasterDataAsync(dataContext, systemOptions.IsInitData,
                    systemOptions.IsQuickDebug, tenantOptions).Wait();
                Thread.Sleep(500);
                SeedService.InitLogData(dataContext, systemOptions.LogDataBase).Wait();
                if (tenantOptions.Enabled && tenantOptions.Type == TenantType.Db)
                {
                    Thread.Sleep(500);
                    SeedService.InitTenantDataAsync(dataContext).Wait();
                }
            }
        }
        catch (Exception e)
        {
            Logger.Error($"Error when creating database initialization data:\n{e.Message}");
            throw;
        }
    }
}
