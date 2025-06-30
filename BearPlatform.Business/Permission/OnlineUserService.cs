using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.WebApp;
using BearPlatform.IBusiness.Permission;
using BearPlatform.Models;
using BearPlatform.ViewModel.Core.Permission.User;
using BearPlatform.ViewModel.Jwt;
using IP2Region.Net.Abstractions;
using Shyjus.BrowserDetection;

namespace BearPlatform.Business.Permission;

/// <summary>
/// 在线用户service
/// </summary>
public class OnlineUserService : IOnlineUserService
{
    #region 字段

    private readonly IBrowserDetector _browserDetector;
    private readonly ISearcher _ipSearcher;

    #endregion

    #region 构造函数

    /// <summary>
    /// 
    /// </summary>
    /// <param name="browserDetector"></param>
    /// <param name="searcher"></param>
    public OnlineUserService(IBrowserDetector browserDetector, ISearcher searcher)
    {
        _browserDetector = browserDetector;
        _ipSearcher = searcher;
    }

    #endregion

    #region 基础方法

    /// <summary>
    /// 保存在线用户
    /// </summary>
    /// <param name="jwtUserInfo"></param>
    /// <param name="remoteIp"></param>
    public async Task<LoginUserInfo> SaveLoginUserAsync(JwtUserInfo jwtUserInfo, string remoteIp)
    {
        var onlineUser = new LoginUserInfo
        {
            UserId = jwtUserInfo.User.Id,
            Account = jwtUserInfo.User.UserName,
            NickName = jwtUserInfo.User.NickName,
            DeptId = jwtUserInfo.User.DeptId,
            DeptName = jwtUserInfo.User.Dept.Name,
            Ip = remoteIp,
            Address = _ipSearcher.Search(remoteIp),
            OperatingSystem = _browserDetector.Browser?.OS,
            DeviceType = _browserDetector.Browser?.DeviceType,
            BrowserName = _browserDetector.Browser?.Name,
            Version = _browserDetector.Browser?.Version,
            LoginTime = DateTime.Now,
            IsAdmin = jwtUserInfo.User.IsAdmin,
            TenantId = jwtUserInfo.User.TenantId
        };
        return await Task.FromResult(onlineUser);
    }

    /// <summary>
    /// 创建Jwt对象
    /// </summary>
    /// <param name="user"></param>
    /// <param name="permissionRoles"></param>
    /// <returns></returns>
    public async Task<JwtUserInfo> CreateJwtUserAsync(UserInfo user, List<string> permissionRoles)
    {
        var jwtUser = new JwtUserInfo
        {
            User = user,
            DataScopes = new List<string>(),
            Roles = permissionRoles
        };
        return await Task.FromResult(jwtUser);
    }

    #endregion
}
