using System;

namespace BearPlatform.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MapFromAttribute : Attribute
{
    public MapFromAttribute(Type fromType)
    {
        FromType = fromType;
    }

    public Type FromType { get; }
}
