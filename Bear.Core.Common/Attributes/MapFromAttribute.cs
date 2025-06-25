using System;

namespace Bear.Core.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class MapFromAttribute : Attribute
{
    public MapFromAttribute(Type fromType)
    {
        FromType = fromType;
    }

    public Type FromType { get; }
}
