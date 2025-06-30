using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BearPlatform.Common.Attributes;

public class BaseActionFilter : Attribute, IAsyncActionFilter
{
    protected HttpContext HttpContext;

    public virtual async Task OnActionExecuting(ActionExecutingContext context)
    {
        await Task.CompletedTask;
    }

    public virtual async Task OnActionExecuted(ActionExecutedContext context)
    {
        await Task.CompletedTask;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        await OnActionExecuting(context);
        if (context.Result == null)
        {
            var nextContext = await next();
            await OnActionExecuted(nextContext);
        }
    }

}
