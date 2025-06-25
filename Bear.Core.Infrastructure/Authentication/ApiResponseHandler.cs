using System.Text.Encodings.Web;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Global;
using Bear.Core.Common.Model;
using Bear.Core.Common.Models;
using Bear.Core.Common.WebApp;
using Bear.Core.Core;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Bear.Core.Infrastructure.Authentication;

public class ApiResponseHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public ApiResponseHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder) :
        base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        throw new NotImplementedException();
    }

    protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.ContentType = "application/json";
        Response.StatusCode = StatusCodes.Status401Unauthorized;
        //await Response.WriteAsync(new ActionResultVm
        //{
        //    Status = StatusCodes.Status401Unauthorized,
        //    ActionError = new ActionError(),
        //    Message = App.L.R("Sys.HttpUnauthorized"),
        //    Path = App.HttpContext?.Request.Path.Value?.ToLower()
        //}.ToJson());

        await Response.WriteAsync(ExcutedResult.FailedResult(App.L.R("Sys.HttpUnauthorized") + App.HttpContext?.Request.Path.Value?.ToLower(), StatusCodes.Status401Unauthorized).ToJson());
    }

    protected override async Task HandleForbiddenAsync(AuthenticationProperties properties)
    {
        var loginUserInfo = await App.Cache.GetAsync<LoginUserInfo>(
            GlobalConstants.CachePrefix.OnlineKey +
            App.HttpUser.JwtToken.ToMd5String16());
        if (loginUserInfo.IsNull())
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status401Unauthorized;
            //await Response.WriteAsync(new ActionResultVm
            //{
            //    Status = StatusCodes.Status401Unauthorized,
            //    ActionError = new ActionError(),
            //    Message = App.L.R("Sys.HttpUnauthorized"),
            //    Path = App.HttpContext?.Request.Path.Value?.ToLower()
            //}.ToJson());
            await Response.WriteAsync(ExcutedResult.FailedResult(App.L.R("Sys.HttpUnauthorized") + App.HttpContext?.Request.Path.Value?.ToLower(), StatusCodes.Status401Unauthorized).ToJson());
        }
        else
        {
            Response.ContentType = "application/json";
            Response.StatusCode = StatusCodes.Status403Forbidden;
            //await Response.WriteAsync(new ActionResultVm
            //{
            //    Status = StatusCodes.Status403Forbidden,
            //    ActionError = new ActionError(),
            //    Message = App.L.R("Sys.HttpForbidden"),
            //    Path = App.HttpContext?.Request.Path.Value?.ToLower()
            //}.ToJson());
            await Response.WriteAsync(ExcutedResult.FailedResult(App.L.R("Sys.HttpForbidden") + App.HttpContext?.Request.Path.Value?.ToLower(), StatusCodes.Status403Forbidden).ToJson());
        }
    }
}
