using BearPlatform.Common.Extensions;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using Microsoft.Extensions.DependencyInjection;

namespace BearPlatform.Infrastructure.Extensions;

/// <summary>
/// 跨域启动器
/// </summary>
public static class CorsSetup
{
    public static void AddCorsSetup(this IServiceCollection services)
    {
        if (services.IsNull()) throw new ArgumentNullException(nameof(services));

        var options = App.GetOptions<CorsOptions>();
        services.AddCors(c =>
        {
            if (options.EnableAll)
            {
                //允许任意跨域请求
                c.AddPolicy(options.Name,
                    policy =>
                    {
                        policy
                            .SetIsOriginAllowed(host => true)
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                    });
            }
            else
            {
                c.AddPolicy(options.Name,
                    policy =>
                    {
                        policy
                            .WithOrigins(options.Policy.Select(x => x.Domain).ToArray())
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            }
        });
    }
}
