using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using BearPlatform.Common.MultiLanguage.Resources;

namespace BearPlatform.Common.Attributes;

/// <summary>
/// 集合必须至少包含一个项目。
/// </summary>
public class AtLeastOneItemAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        // 获取当前属性的元数据
        var property = validationContext.ObjectType.GetProperty(validationContext.MemberName ?? string.Empty);
        var displayAttribute = property?.GetCustomAttribute<DisplayAttribute>();
        var displayName = displayAttribute?.Name ?? validationContext.MemberName;

        // 验证逻辑：检查集合是否为空
        if (value is IEnumerable list && list.Cast<object>().Any())
        {
            return ValidationResult.Success;
        }

        return new ValidationResult(Localizer.R("{0}AtLeastOne", Localizer.R(displayName)));
    }
}
