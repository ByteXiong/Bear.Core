using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.WebApp;
using BearPlatform.Models;
using BearPlatform.ViewModel.Core.Permission.User;
using BearPlatform.ViewModel.Jwt;

namespace BearPlatform.IBusiness.Permission;

/// <summary>
/// 在线用户接口
/// </summary>
public interface IOnlineUserService
{
    #region 基础接口

    /// <summary>
    /// 保存在线用户
    /// </summary>
    /// <param name="jwtUserInfo"></param>
    /// <param name="remoteIp"></param>
    Task<LoginUserInfo> SaveLoginUserAsync(JwtUserInfo jwtUserInfo, string remoteIp);

    /// <summary>
    /// jwt用户信息
    /// </summary>
    /// <param name="user"></param>
    /// <param name="permissionRoles"></param>
    /// <returns></returns>
    Task<JwtUserInfo> CreateJwtUserAsync(UserInfo user, List<string> permissionRoles);

    #endregion
}
