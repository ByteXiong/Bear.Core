using System.Reflection;
using Bear.Core.Common.Attributes;
using Bear.Core.Common.Global;
using Bear.Core.Core.ConfigOptions.Core;
using Microsoft.Extensions.DependencyInjection;

namespace Bear.Core.Infrastructure.Extensions;

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
