using System;
using Castle.DynamicProxy;

namespace Bear.Core.Common.DI;

public abstract class BaseFilterAttribute : Attribute, IFilter
{
    public abstract void OnActionExecuted(IInvocation invocation);
    public abstract void OnActionExecuting(IInvocation invocation);
}
