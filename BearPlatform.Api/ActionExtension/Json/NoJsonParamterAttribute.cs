using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BearPlatform.Api.ActionExtension.Json;

/// <summary>
/// 忽略Json参数支持
/// </summary>
public class NoJsonParamterAttribute : Attribute, IActionFilter
{
    /// <summary>
    /// Action执行之前执行
    /// </summary>
    /// <param name="context">过滤器上下文</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }

    /// <summary>
    /// Action执行完毕之后执行
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
