using BearPlatform.Common.Extensions;
using BearPlatform.Core.SeedData;
using Microsoft.Extensions.DependencyInjection;

namespace BearPlatform.Infrastructure.Extensions;

/// <summary>
/// 数据库上下文启动器
/// </summary>
public static class DbSetup
{
    public static void AddDbSetup(this IServiceCollection services)
    {
        if (services.IsNull()) throw new ArgumentNullException(nameof(services));

        services.AddScoped<SeedService>();
        services.AddScoped<DataContext>();
    }
}
