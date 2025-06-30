using System;
using System.Threading.Tasks;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Core;
using BearPlatform.Core.Utils;
using BearPlatform.IBusiness.System;
using BearPlatform.Models.Dto.Core.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace BearPlatform.Api.ActionExtension.Sign;

/// <summary>
/// 接口对外签名验证
/// </summary>
public class VerifySignatureAttribute : BaseActionFilter
{
    /// <summary>
    /// Action执行之前执行
    /// </summary>
    /// <param name="filterContext"></param>
    public override async Task OnActionExecuting(ActionExecutingContext filterContext)
    {
        //判断是否需要签名
        if (filterContext.ContainsFilter<IgnoreVerifySignatureAttribute>())
            return;
        HttpContext = filterContext.HttpContext;
        var request = filterContext.HttpContext.Request;

        var appId = request.Headers["appId"].ToString();
        if (appId.IsNullOrEmpty())
        {
            filterContext.Result = Error(App.L.R("{0}required", "appId"));
            return;
        }

        var time = request.Headers["time"].ToString();
        if (time.IsNullOrEmpty())
        {
            filterContext.Result = Error(App.L.R("{0}required", "time"));
            return;
        }

        if (time.ToDateTime() < DateTime.Now.AddMinutes(-5) || time.ToDateTime() > DateTime.Now.AddMinutes(5))
        {
            filterContext.Result = Error("time过期");
            return;
        }

        var guid = request.Headers["guid"].ToString();
        if (guid.IsNullOrEmpty())
        {
            filterContext.Result = Error(App.L.R("{0}required", "guid"));
            return;
        }

        var guidKey = $"ApiGuid_{guid}";
        if (!App.Cache.GetAsync<string>(guidKey).IsNullOrEmpty())
        {
            await App.Cache.SetAsync(guidKey, "1", null, null);
        }
        else
        {
            filterContext.Result = Error(App.L.R("Error.DuplicateCallNotAllowed"));
            return;
        }

        request.EnableBuffering();
        var body = request.Body.ReadToString();

        var sign = request.Headers["sign"].ToString();
        if (sign.IsNullOrEmpty())
        {
            filterContext.Result = Error(App.L.R("{0}required", "sign"));
            return;
        }

        var appSecretModel = await filterContext.HttpContext.RequestServices.GetRequiredService<IAppSecretService>()
            .TableWhere(x => x.AppId == appId).FirstAsync();
        if (appSecretModel.IsNull())
        {
            CreateUpdateAppSecretDto createUpdateAppSecretDto = new CreateUpdateAppSecretDto
            {
                AppId = appId
            };
            filterContext.Result =
                Error(ValidationError.NotExist(createUpdateAppSecretDto, LanguageKeyConstants.AppSecret,
                    nameof(createUpdateAppSecretDto.AppId)));
            return;
        }

        var newSign = BuildApiSign(appId, appSecretModel.AppSecretKey, guid, time.ToDateTime(), body);
        if (sign != newSign)
        {
            var msg =
                $@"Signature error!
                        headers:{request.Headers.ToJson()}
                        body:{body}
                        Pass in sign{sign}
                        Correct sign:{newSign}";
            filterContext.Result = Error(msg);
        }
    }

    /// <summary>
    /// 生成接口签名sign
    /// 注：md5(appId+time+guid+body+appSecret)
    /// </summary>
    /// <param name="appId">应用Id</param>
    /// <param name="appSecret">应用密钥</param>
    /// <param name="guid">唯一GUID</param>
    /// <param name="time">时间</param>
    /// <param name="body">请求体</param>
    /// <returns></returns>
    private string BuildApiSign(string appId, string appSecret, string guid, DateTime time, string body)
    {
        return $"{appId}{time:yyyy-MM-dd HH:mm:ss}{guid}{body}{appSecret}".ToMd5String();
    }
}
