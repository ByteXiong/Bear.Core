using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BearPlatform.Common.ClassLibrary;

public class CustomContractResolver : CamelCasePropertyNamesContractResolver
{
    ///// <summary>
    ///// 实现首字母小写
    ///// </summary>
    ///// <param name="propertyName"></param>
    ///// <returns></returns>
    protected override string ResolvePropertyName(string propertyName)
    {
        return propertyName.Substring(0, 1).ToLower() + propertyName.Substring(1);
    }

    /// <summary>
    /// 对长整型做处理
    /// </summary>
    /// <param name="objectType"></param>
    /// <returns></returns>
    protected override JsonConverter ResolveContractConverter(Type objectType)
    {
        if (objectType == typeof(long))
        {
            return new JsonConverterLong();
        }

        return base.ResolveContractConverter(objectType);
    }



    /// <summary>
    /// 对字符串做处理
    /// </summary>
    /// <param name="member"></param>
    /// <param name="memberSerialization"></param>
    /// <returns></returns>
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);
        if (property.PropertyType == typeof(string))
        {
            // 替换 ValueProvider
            property.ValueProvider = new NullToEmptyStringValueProvider(member);
        }
        return property;
    }
}

public class NullToEmptyStringValueProvider : IValueProvider
{
    private readonly MemberInfo _member;

    public NullToEmptyStringValueProvider(MemberInfo member)
    {
        _member = member;
    }

    public object GetValue(object target)
    {
        object value = null;
        switch (_member.MemberType)
        {
            case MemberTypes.Property:
                value = ((PropertyInfo)_member).GetValue(target);
                break;
            case MemberTypes.Field:
                value = ((FieldInfo)_member).GetValue(target);
                break;
        }
        return value ?? ""; // 关键：将 null 转为 ""
    }

    public void SetValue(object target, object value)
    {
        // 反序列化时可选择是否将 "" 转回 null
        // 此处保持原值
        switch (_member.MemberType)
        {
            case MemberTypes.Property:
                ((PropertyInfo)_member).SetValue(target, value);
                break;
            case MemberTypes.Field:
                ((FieldInfo)_member).SetValue(target, value);
                break;
        }
    }
}
