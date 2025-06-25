using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bear.Core.Common.Attributes;
using Bear.Core.Common.Enums;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Global;
using Bear.Core.Common.Model;
using Bear.Core.Entity;
using Bear.Core.Entity.Core.Permission;
using Bear.Core.Entity.Core.Permission.Role;
using Bear.Core.Entity.Core.Permission.User;
using Bear.Core.Entity.Permission;
using Bear.Core.IBusiness.Permission;
using Bear.Core.ViewModel.Jwt;

namespace Bear.Core.Business.Permission;

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
            .Where((ur, rm, m) => ur.UserId == userId && m.MenuType != MenuType.Directory && m.Permission != null)
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
