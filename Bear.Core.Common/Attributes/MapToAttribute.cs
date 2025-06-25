using System;

namespace Bear.Core.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MapToAttribute : Attribute
{
    public MapToAttribute(Type targetType)
    {
        TargetType = targetType;
    }

    public Type TargetType { get; }
}
