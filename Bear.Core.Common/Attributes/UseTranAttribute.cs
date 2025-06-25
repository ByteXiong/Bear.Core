using System;

namespace Bear.Core.Common.Attributes;

/// <summary>
/// 事务特性 AOP拦截使用
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public class UseTranAttribute : Attribute
{
}
