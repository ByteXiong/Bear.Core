using System;

namespace BearPlatform.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MapToAttribute : Attribute
{
    public MapToAttribute(Type targetType)
    {
        TargetType = targetType;
    }

    public Type TargetType { get; }
}
