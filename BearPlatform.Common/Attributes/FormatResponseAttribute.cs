using System.Threading.Tasks;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BearPlatform.Common.Attributes
{
    /// <summary>
    /// 若Action返回对象为自定义对象,则将其转为JSON
    /// </summary>
    public class FormatResponseAttribute : BaseActionFilter
    {
        public override async Task OnActionExecuted(ActionExecutedContext context)
        {
            if (context.ContainsFilter<NoFormatResponseAttribute>())
                return;

            if (context.Result is EmptyResult)
                context.Result = new ObjectResult(ExcutedResult.SuccessResult());
            else if (context.Result is ObjectResult res)
            {
                if (res.Value is ExcutedResult)

                    context.Result = new ObjectResult(res.Value);
                else

                    context.Result = Result1(res.Value);
            }

            await Task.CompletedTask;





        }


        public IActionResult Result1(object obj)//实例化对象
        {
            ObjectResult result = new ObjectResult(ExcutedResult<object>.SuccessResult(obj));
            return result;
        }
    }
}
