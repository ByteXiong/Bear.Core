using System.Reflection;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Global;
using BearPlatform.Core.ConfigOptions.Core;
using Microsoft.Extensions.DependencyInjection;

namespace BearPlatform.Infrastructure.Extensions;

public static class OptionRegisterSetup
{
    /// <summary>
    /// 注册配置选项
    /// </summary>
    /// <param name="services"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public static void AddOptionRegisterSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var optionTypes = GlobalType.CoreTypes
            .Where(x => x.GetCustomAttribute<OptionsSettingsAttribute>() != null).ToList();

        foreach (var optionType in optionTypes)
        {
            services.AddConfigurableOptions(optionType);
        }
    }
}
