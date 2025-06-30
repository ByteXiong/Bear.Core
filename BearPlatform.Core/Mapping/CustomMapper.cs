using System.Reflection;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Global;
using Mapster;

namespace BearPlatform.Core.Mapping;

/// <summary>
/// 对象映射
/// </summary>
public class CustomMapper : IRegister
{
    /// <summary>
    /// 注册
    /// </summary>
    /// <param name="config"></param>
    public void Register(TypeAdapterConfig config)
    {
        var viewModelTypes = GlobalType.ViewModelTypes
            .Where(x => x.GetCustomAttribute<AutoMappingAttribute>() != null)
            .Select(x => x.GetCustomAttribute<AutoMappingAttribute>());

        var dtoTypes = GlobalType.SharedModelTypes
            .Where(x => x.GetCustomAttribute<AutoMappingAttribute>() != null)
            .Select(x => x.GetCustomAttribute<AutoMappingAttribute>());

        List<(Type sourceType, Type targetType)> maps = (from attribute in viewModelTypes
                                                         where attribute != null
                                                         select (attribute.SourceType, attribute.TargetType)).ToList();
        maps.AddRange(from attribute in dtoTypes
                      where attribute != null
                      select (attribute.TargetType, attribute.SourceType));


        //根据AutoMappingAttribute特性自动映射
        maps.ForEach(aMap => { config.NewConfig(aMap.sourceType, aMap.targetType); });

        //自定义映射 会覆盖存在的
        // config.NewConfig<User, UserDto>() .Ignore(dest => dest.Password)
        //     .Map(dest => dest.Dept123, src => src.Dept.Adapt<DeptSmallDto>());
    }
}
