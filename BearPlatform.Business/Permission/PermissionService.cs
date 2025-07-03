using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Model;
using BearPlatform.Entity;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Entity.Core.Permission.Role;
using BearPlatform.Entity.Core.Permission.User;
using BearPlatform.Entity.Permission;
using BearPlatform.IBusiness.Permission;
using BearPlatform.ViewModel.Jwt;

namespace BearPlatform.Business.Permission;

/// <summary>
/// 权限服务
/// </summary>
public class PermissionService : BaseServices<Role>, IPermissionService
{
    #region 基础方法

    /// <summary>
    /// 获取权限标识符
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [UseCache(Expiration = 60, KeyPrefix = GlobalConstants.CachePrefix.UserPermissionRoles)]
    public async Task<List<string>> GetPermissionIdentifierAsync(long userId)
    {
        var permissionIdentifierList = await SugarClient
            .Queryable<UserRole, RoleMenu, Menu>((ur, rm, m) => ur.RoleId == rm.RoleId && rm.MenuId == m.Id)
            .GroupBy((ur, rm, m) => m.Permission)
            .Where((ur, rm, m) => ur.UserId == userId && m.MenuType != MenuTypeEnum.Directory && m.Permission != null)
            .OrderBy((ur, rm, m) => m.Permission)
            .ClearFilter<ICreateByEntity>()
            .Select((ur, rm, m) => m.Permission).ToListAsync();
        permissionIdentifierList = permissionIdentifierList.Where(x => !x.IsNullOrEmpty()).ToList();
        return permissionIdentifierList;
    }


    /// <summary>
    /// 获取权限urls
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    [UseCache(Expiration = 60, KeyPrefix = GlobalConstants.CachePrefix.UserPermissionUrls)]
    public async Task<List<UrlAccessControlVo>> GetUrlAccessControlAsync(long userId)
    {
        var urlAccessControlList = await SugarClient
            .Queryable<UserRole, RoleApis, Apis>((ur, ra, a) => ur.RoleId == ra.RoleId && ra.ApisId == a.Id)
            .GroupBy((ur, ra, a) => new { a.Url, a.Method })
            .Where(ur => ur.UserId == userId)
            .OrderBy((ur, ra, a) => a.Url)
            .ClearFilter<ICreateByEntity>()
            .Select((ur, ra, a) => new UrlAccessControlVo
            {
                Url = a.Url,
                Method = a.Method
            }).ToListAsync();
        urlAccessControlList = urlAccessControlList.Where(x => !x.IsNullOrEmpty()).ToList();
        return urlAccessControlList;
    }

    #endregion
}
