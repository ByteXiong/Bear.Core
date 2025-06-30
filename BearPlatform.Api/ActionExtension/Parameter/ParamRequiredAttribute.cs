using System;
using System.Collections.Generic;
using System.Linq;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BearPlatform.Api.ActionExtension.Parameter;

/// <summary>
/// 参数必填
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class ParamRequiredAttribute(params string[] parameters) : Attribute, IActionFilter
{
    private readonly List<string> _parameters = parameters.ToList();

    /// <summary>
    /// Action执行之前执行
    /// </summary>
    /// <param name="filterContext">过滤器上下文</param>
    public void OnActionExecuting(ActionExecutingContext filterContext)
    {
        var allParameters = HttpHelper.GetAllRequestParams(filterContext.HttpContext);
        var needParameters = _parameters.Where(x => !allParameters.ContainsKey(x) || allParameters[x].IsNullOrEmpty())
            .ToList();
        if (needParameters.Count == 0) return;
        var service = filterContext.HttpContext.RequestServices.GetService<IHttpContextAccessor>();
        var res = new ActionResultVm
        {
            Status = StatusCodes.Status400BadRequest,
            Timestamp = DateTime.Now.ToUnixTimeStampMillisecond().ToString(),
            Message = App.L.R("{0}required", string.Join(",", needParameters)),
            Path = service?.HttpContext?.Request.Path.Value?.ToLower()
        };
        filterContext.Result = new ContentResult
        {
            StatusCode = StatusCodes.Status400BadRequest,
            Content = res.ToJson(),
            ContentType = "application/json;charset=utf-8"
        };
    }

    /// <summary>
    /// Action执行完毕之后执行
    /// </summary>
    /// <param name="filterContext"></param>
    public void OnActionExecuted(ActionExecutedContext filterContext)
    {
    }
}
