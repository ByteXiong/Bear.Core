using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Asp.Versioning;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Helper;
using BearPlatform.Common.IdGenerator;
using BearPlatform.Common.WebApp;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.Core.Utils;
using BearPlatform.IBusiness;
using BearPlatform.IBusiness.Permission;
using BearPlatform.IBusiness.Queued;
using BearPlatform.IBusiness.System;
using BearPlatform.Infrastructure.Authentication;
using BearPlatform.Models;
using BearPlatform.Models.Queries.Login;
using BearPlatform.ViewModel.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IOnlineUserService = BearPlatform.IBusiness.Permission.IOnlineUserService;

namespace BearPlatform.Api.Controllers.Auth;

/// <summary>
/// 授权管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class LoginController : BaseApiController
{
    #region 构造函数

    public LoginController(IUserService userService, IPermissionService permissionService,
        IBusiness.Permission.IOnlineUserService onlineUserService, IQueuedEmailService queuedEmailService,
        ITokenService tokenService, ITokenBlacklistService tokenBlacklistService)
    {
        _userService = userService;
        _permissionService = permissionService;
        _onlineUserService = onlineUserService;
        _queuedEmailService = queuedEmailService;
        _tokenService = tokenService;
        _tokenBlacklistService = tokenBlacklistService;
    }

    #endregion

    #region 字段

    private readonly IUserService _userService;
    private readonly IPermissionService _permissionService;
    private readonly IOnlineUserService _onlineUserService;
    private readonly IQueuedEmailService _queuedEmailService;
    private readonly ITokenService _tokenService;
    private readonly ITokenBlacklistService _tokenBlacklistService;

    #endregion

    #region 内部接口

    /// <summary>
    /// 获取验证码
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [AllowAnonymous]
    [NotAudit]
    [ApiVersion((int)VersionEnum.App, Deprecated = false)]
    [ApiVersion((int)VersionEnum.Pc, Deprecated = false)]
    public async Task<CaptchaVo> Captcha()
    {
        var showCaptcha = true; //是否显示验证码
        var captchaOptions = App.GetOptions<CaptchaOptions>();
        if (captchaOptions.Threshold > 0)
        {
            var thresholdCacheKey =
                GlobalConstants.CachePrefix.Threshold + App.HttpContext.Connection.RemoteIpAddress;
            var failedThreshold = await App.Cache.GetAsync<int>(thresholdCacheKey);
            if (failedThreshold <= 0)
            {
                failedThreshold = 1;
                await App.Cache.SetAsync(thresholdCacheKey, failedThreshold,
                    TimeSpan.FromSeconds(captchaOptions.TimeOut), null);
            }

            showCaptcha = failedThreshold > captchaOptions.Threshold;
        }


        var (imgBytes, code) = SixLaborsImageHelper.BuildVerifyCode(captchaOptions.ImgWidth,
            captchaOptions.ImgHeight,
            captchaOptions.FontSize, captchaOptions.KeyLength);
        var img = ImgHelper.ToBase64StringUrl(imgBytes);
        var captchaId = GlobalConstants.CachePrefix.CaptchaId +
                        IdHelper.NextId().ToString().Base64Encode();
        await App.Cache.SetAsync(captchaId, code, TimeSpan.FromMinutes(2), null);
        var captchaVo = new CaptchaVo
        {
            Img = img,
            CaptchaId = captchaId,
            ShowCaptcha = showCaptcha
        };

        return captchaVo;
    }


    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="authUser"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiVersion((int)VersionEnum.App, Deprecated = false)]
    [ApiVersion((int)VersionEnum.Pc, Deprecated = false)]
    public async Task<LoginToken> LoginAsync([FromBody] LoginParam authUser)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            throw new BusException(actionError.ToString());
        }


        var loginFailedLimitOptions = App.GetOptions<LoginFailedLimitOptions>();
        var attempsCacheKey = GlobalConstants.CachePrefix.Attempts + App.HttpContext.Connection.RemoteIpAddress +
                              authUser.UserName;
        LoginAttempt loginAttempt = null;
        if (loginFailedLimitOptions.Enabled)
        {
            loginAttempt = await App.Cache.GetAsync<LoginAttempt>(attempsCacheKey);
            if (loginAttempt.IsNull())
            {
                loginAttempt = new LoginAttempt
                {
                    Count = 0,
                    IsLocked = false,
                    LockUntil = DateTime.MinValue
                };
                await App.Cache.SetAsync(attempsCacheKey, loginAttempt,
                    TimeSpan.FromSeconds(loginFailedLimitOptions.Lockout), null);
            }

            if (loginAttempt.IsLocked && DateTime.Now < loginAttempt.LockUntil)
            {
                // 可以实施账户锁定时，通过邮件或短信通知用户。
                // 可以实施账户锁定后要求管理员手动解锁
                throw new BusException(App.L.R("Error.AccountLockedWithUnlockTime{0}", loginAttempt.LockUntil.ToString("yyyy-MM-dd HH:mm:ss")));
            }
        }


        var captchaOptions = App.GetOptions<CaptchaOptions>();
        var showCaptcha = true; //是否显示验证码
        var thresholdCacheKey = GlobalConstants.CachePrefix.Threshold + App.HttpContext.Connection.RemoteIpAddress;
        var failedThreshold = 0;
        if (captchaOptions.Threshold > 0)
        {
            failedThreshold = await App.Cache.GetAsync<int>(thresholdCacheKey);
            if (failedThreshold <= 0)
            {
                failedThreshold = 1;
                await App.Cache.SetAsync(thresholdCacheKey, failedThreshold,
                    TimeSpan.FromSeconds(captchaOptions.TimeOut), null);
            }

            showCaptcha = failedThreshold > captchaOptions.Threshold;
        }


        if (!App.GetOptions<SystemOptions>().IsQuickDebug && showCaptcha)
        {
            if (authUser.Captcha.IsNullOrEmpty())
            {
                throw new BusException(ValidationError.Required(authUser, nameof(authUser.Captcha)));
            }

            if (authUser.CaptchaId.IsNullOrEmpty())
            {
                throw new BusException(ValidationError.Required(authUser, nameof(authUser.CaptchaId)));
            }


            var code = await App.Cache.GetAsync<string>(authUser.CaptchaId);
            if (code.IsNullOrEmpty())
            {
                throw new BusException(App.L.R("Error.VerificationCodeExpired"));
            }

            if (!code.Equals(authUser.Captcha))
            {
                if (captchaOptions.Threshold > 0)
                {
                    failedThreshold++;
                    await App.Cache.SetAsync(thresholdCacheKey, failedThreshold,
                        TimeSpan.FromSeconds(captchaOptions.TimeOut),
                        null);
                }

                throw new BusException(App.L.R("Error.InvalidVerificationCode"));
            }
        }

        var userDto = await _userService.QueryByNameAsync(authUser.UserName);
        if (userDto == null)
        {
            if (captchaOptions.Threshold > 0)
            {
                failedThreshold++;
                await App.Cache.SetAsync(thresholdCacheKey, failedThreshold,
                    TimeSpan.FromSeconds(captchaOptions.TimeOut),
                    null);
            }

            throw new BusException(App.L.R("Error.UserNotFound"));
        }

        var rsaOptions = App.GetOptions<RsaOptions>();
        var password = new RsaHelper(rsaOptions.PrivateKey, rsaOptions.PublicKey).Decrypt(authUser.Password);
        if (!BCryptHelper.Verify(password, userDto.Password))
        {
            if (captchaOptions.Threshold > 0)
            {
                failedThreshold++;
                await App.Cache.SetAsync(thresholdCacheKey, failedThreshold,
                    TimeSpan.FromSeconds(captchaOptions.TimeOut),
                    null);
            }

            if (loginFailedLimitOptions.Enabled && loginAttempt != null)
            {
                loginAttempt.Count++;
                if (loginAttempt.Count >= loginFailedLimitOptions.MaxAttempts)
                {
                    loginAttempt.IsLocked = true;
                    loginAttempt.LockUntil = DateTime.Now.AddSeconds(loginFailedLimitOptions.Lockout);
                }


                await App.Cache.SetAsync(attempsCacheKey, loginAttempt,
                    TimeSpan.FromSeconds(loginFailedLimitOptions.Lockout), null);
            }

            return loginFailedLimitOptions.Enabled
                ? throw new BusException(App.L.R("Error.InvalidPasswordWithLockWarning"))
                : throw new BusException(App.L.R("Error.InvalidPassword"));
        }

        if (!userDto.Enabled)
        {
            if (captchaOptions.Threshold > 0)
            {
                failedThreshold++;
                await App.Cache.SetAsync(thresholdCacheKey, failedThreshold,
                    TimeSpan.FromSeconds(captchaOptions.TimeOut),
                    null);
            }

            throw new BusException(App.L.R("Error.UserNotActivated"));
        }

        await App.Cache.RemoveAsync(authUser.CaptchaId);
        await App.Cache.RemoveAsync(thresholdCacheKey);
        await App.Cache.RemoveAsync(attempsCacheKey);
        var netUser = await _userService.GetInfoAsync(userDto.Id);
        var result = await LoginResult(netUser, "login");
        return result.LoginToken;
    }


    /// <summary>
    /// 刷新Token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [ApiVersion((int)VersionEnum.App, Deprecated = false)]
    [ApiVersion((int)VersionEnum.Pc, Deprecated = false)]
    public async Task<LoginToken> RefreshToken(string token)
    {
        var tokenMd5 = token.ToMd5String16();
        var tokenBlacklist = await _tokenBlacklistService
            .TableWhere(x => x.AccessToken == tokenMd5, null, null, null, true)
            .FirstAsync();
        if (!tokenBlacklist.IsNull())
        {
            throw new BusException(App.L.R("Error.TokenRevoked"));
        }

        var jwtSecurityToken = await _tokenService.ReadJwtToken(token);
        if (jwtSecurityToken != null)
        {
            var userId = Convert.ToInt64(jwtSecurityToken.Claims
                .FirstOrDefault(s => s.Type == AuthConstants.JwtClaimTypes.Jti)?.Value);
            var loginTime = Convert.ToInt64(jwtSecurityToken.Claims
                .FirstOrDefault(s => s.Type == AuthConstants.JwtClaimTypes.Iat)?.Value).TicksToDateTime();
            var nowTime = DateTime.Now.ToLocalTime();
            var refreshTime = loginTime.AddHours(App.GetOptions<JwtAuthOptions>().RefreshTokenExpires);
            // 允许token刷新时间内
            if (nowTime <= refreshTime)
            {
                var netUser = await _userService.GetInfoAsync(userId);
                if (netUser.IsNotNull())
                    if (netUser.UpdateTime == null || netUser.UpdateTime < loginTime) {
                        var result = await LoginResult(netUser, "refresh");
                        return result.LoginToken;
                    }
                      
            }

            throw new BusException(App.L.R("Error.TokenExpired"),401);
        }

        throw new BusException(App.L.R("Error.TokenParseFailed"), 401);
    }

    /// <summary>
    /// 获取当前登录用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [NotAudit]
    [ApiVersion((int)VersionEnum.App, Deprecated = false)]
    [ApiVersion((int)VersionEnum.Pc, Deprecated = false)]
    public async Task<JwtUserInfo> GetInfo()
    {
        var netUser = await _userService.GetInfoAsync(App.HttpUser.Id);
        var permissionIdentifierList = await _permissionService.GetPermissionIdentifierAsync(netUser.Id);
        permissionIdentifierList.AddRange(netUser.Roles.Select(r => r.Permission));
        var jwtUserInfo = await _onlineUserService.CreateJwtUserAsync(netUser, permissionIdentifierList);
        return jwtUserInfo;
    }


    ///// <summary>
    ///// 获取验证码，申请变更邮箱
    ///// </summary>
    ///// <returns></returns>
    //[HttpPost]
    //[ParamRequired("email")]
    //public async Task<OperateResult> ResetEmail(string email)
    //{
    //    if (!email.IsEmail())
    //    {
    //        throw new BusException(App.L.R("{0}Error.Format",
    //            App.L.R("Sys.Email")));
    //    }

    //    var result = await _queuedEmailService.ResetEmail(email, "EmailVerificationCode");
    //    return result;
    //}


    ///// <summary>
    ///// 系统用户登出
    ///// </summary>
    ///// <returns></returns>
    //[HttpDelete]
    //[AllowAnonymous]
    //public async Task Logout()
    //{
    //    //清理缓存
    //    if (App.HttpUser.IsNotNull())
    //    {
    //        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.OnlineKey +
    //                                    App.HttpUser.JwtToken.ToMd5String16());
    //        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserInfoById +
    //                                    App.HttpUser.Id.ToString().ToMd5String16());
    //        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserMenuById +
    //                                    App.HttpUser.Id.ToString().ToMd5String16());
    //        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserPermissionRoles +
    //                                    App.HttpUser.Id.ToString().ToMd5String16());
    //        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserPermissionUrls +
    //                                    App.HttpUser.Id.ToString().ToMd5String16());
    //        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserDataScopeById +
    //                                    App.HttpUser.Id.ToString().ToMd5String16());
    //    }

    //}


    ///// <summary>
    ///// swagger登录
    ///// </summary>
    ///// <param name="swaggerLoginAuthUser"></param>
    ///// <returns></returns>
    //[HttpPost]
    //[AllowAnonymous]
    //public async Task SwaggerLogin([FromBody] SwaggerLoginAuthUser swaggerLoginAuthUser)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        var actionError = ModelState.GetErrors();
    //        throw new BusException(actionError.ToString());
    //    }

    //    var userDto = await _userService.QueryByNameAsync(swaggerLoginAuthUser.UserName);
    //    if (userDto == null)
    //    {
    //        throw new BusException(App.L.R("Error.UserNotFound"));
    //    }

    //    var rsaOptions = App.GetOptions<RsaOptions>();
    //    var password =
    //        new RsaHelper(rsaOptions.PrivateKey, rsaOptions.PublicKey).Decrypt(swaggerLoginAuthUser.Password);
    //    if (!BCryptHelper.Verify(password, userDto.Password))
    //    {
    //        throw new BusException(App.L.R("Error.InvalidPassword"));
    //    }


    //    if (!userDto.Enabled)
    //    {
    //        throw new BusException(App.L.R("Error.UserNotActivated"));
    //    }

    //    App.HttpContext.Session.SetInt32("swagger-key", 1);
    //}

    #endregion


    #region 私有方法

    /// <summary>
    /// 登录或刷新token相应结果
    /// </summary>
    /// <param name="user"></param>
    /// <param name="type">login:登录,refresh:刷新token</param>
    /// <returns></returns>
    private async Task<LoginDTO> LoginResult(UserInfo user, string type)
    {
        var permissionIdentifierList = new List<string>();
        var refresh = true;
        if (type.Equals("login"))
        {
            refresh = false;
            permissionIdentifierList = await _permissionService.GetPermissionIdentifierAsync(user.Id);
            permissionIdentifierList.AddRange(user.Roles.Select(r => r.Permission));
        }

        var remoteIp = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var jwtUserInfo = await _onlineUserService.CreateJwtUserAsync(user, permissionIdentifierList);
        var loginUserInfo = await _onlineUserService.SaveLoginUserAsync(jwtUserInfo, remoteIp);
        var token = await _tokenService.IssueTokenAsync(loginUserInfo, refresh);
        loginUserInfo.AccessToken = refresh ? token.RefreshToken : token.AccessToken;
        var onlineKey = loginUserInfo.AccessToken.ToMd5String16();
        await App.Cache.SetAsync(
            GlobalConstants.CachePrefix.OnlineKey + onlineKey,
            loginUserInfo, TimeSpan.FromHours(2), CacheExpireType.Absolute);

        switch (type)
        {
            case "login":
                return new LoginDTO() { LoginToken = token, User = jwtUserInfo };
            case "refresh":

                return new LoginDTO() { LoginToken = token };
            default:
                return new LoginDTO();
        }
    }

    public class LoginDTO
    {
        public JwtUserInfo User { get; set; }
        public LoginToken LoginToken { get; set; }

    }

    #endregion
}
