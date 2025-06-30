using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Core;
using BearPlatform.Entity;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Entity.Core.Permission.Role;
using BearPlatform.Entity.Core.Permission.User;
using BearPlatform.Entity.Permission;
using BearPlatform.IBusiness.Permission;
using BearPlatform.Models.Permission;
using BearPlatform.SqlSugar;
using SqlSugar;

namespace BearPlatform.Business;

/// <summary>
/// 角色服务
/// </summary>
public class RoleService : BaseServices<Role>, IRoleService
{
    #region 字段

    #endregion

    #region 构造函数

    public RoleService()
    {
    }

    #endregion

    #region 基础方法

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<RoleDTO>> GetPageAsync(RoleParam param)
    {
        Expression<Func<Role, bool>> where = x =>true;
        if (!string.IsNullOrWhiteSpace(param.KeyWord))
        {
            param.KeyWord = param.KeyWord.Trim();
            where = where.And(x => x.Name.Contains(param.KeyWord));
        }
        var page = await  GetIQueryable(where).Select<RoleDTO>().ToPagedResultsAsync(param);
        return page;
    }

    [UseTran]
    public async Task<long> AddAsync(UpdateRoleParam param)
    {
        //await VerificationUserRoleLevelAsync(param.Level);
        if (await TableWhere(r => r.Name == param.Name).AnyAsync())
        {
            throw new BusException($"角色名称=>{param.Name}=>已存在!");
        }

        if (await TableWhere(r => r.Permission == param.Permission).AnyAsync())
        {
            throw new BusException($"权限标识=>{param.Permission}=>已存在!");
        }

        if (param.DataScopeType == DataScopeType.Customize && param.Depts.Count == 0)
        {
            throw new BusException("数据权限为自定义,请至少选择一个部门!");
        }

        var role = App.Mapper.MapTo<Role>(param);
        await AddAsync(role);

        if (param.DataScopeType == DataScopeType.Customize && param.Depts.Count != 0)
        {
            var roleDepts = new List<RoleDept>();
            roleDepts.AddRange(param.Depts.Select(rd => new RoleDept
                { RoleId = role.Id, DeptId = rd.Id }));
            await SugarClient.Insertable(roleDepts).ExecuteCommandAsync();
        }

        return param.Id;
    }

    [UseTran]
    public async Task<long> UpdateAsync(UpdateRoleParam param)
    {
        //取出待更新数据
        var oldRole = await TableWhere(x => x.Id == param.Id).Includes(x => x.Users).FirstAsync();
        if (oldRole.IsNull())
        {
            throw new BusException("数据不存在！");
        }

        if (oldRole.Name != param.Name &&
            await TableWhere(x => x.Name == param.Name).AnyAsync())
        {
            throw new BusException($"角色名称=>{param.Name}=>已存在!");
        }

        if (oldRole.Permission != param.Permission &&
            await TableWhere(x => x.Permission == param.Permission).AnyAsync())
        {
            throw new BusException($"权限标识=>{param.Permission}=>已存在!");
        }

        //await VerificationUserRoleLevelAsync(param.Level);
        var role = App.Mapper.MapTo<Role>(param);
        await UpdateAsync(role);

        //删除部门权限关联
        await SugarClient.Deleteable<RoleDept>().Where(x => x.RoleId == role.Id).ExecuteCommandAsync();
        if (!param.Depts.IsNullOrEmpty() && param.Depts.Count > 0)
        {
            var roleDepts = new List<RoleDept>();
            roleDepts.AddRange(param.Depts.Select(rd => new RoleDept
                { RoleId = role.Id, DeptId = rd.Id }));
            await SugarClient.Insertable(roleDepts).ExecuteCommandAsync();
        }

        foreach (var user in oldRole.Users)
        {
            await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserDataScopeById +
                                        user.Id.ToString().ToMd5String16());
        }

        return param.Id;
    }

    [UseTran]
    public async Task<int> DeleteAsync(HashSet<long> ids)
    {
        //返回用户列表的最大角色等级
        var roles = await TableWhere(x => ids.Contains(x.Id)).Includes(x => x.Users).ToListAsync();
        int userCount = 0;
        if (roles.Any(role => role.Users != null && role.Users.Count != 0))
        {
            userCount++;
        }

        if (userCount > 0)
        {
            throw new BusException("存在用户关联，请解除后再试！");
        }


        List<int> levels = new List<int>();
        levels.AddRange(roles.Select(x => x.Level).ToList());
        int minLevel = levels.Min();
        //验证当前用户角色等级是否大于待待删除的角色等级 ，不满足则抛异常
        //await VerificationUserRoleLevelAsync(minLevel);

        //删除角色 角色部门 角色菜单
        return await LogicDeleteAsync<Role>(x => ids.Contains(x.Id));
    }


    //public async Task<int> VerificationUserRoleLevelAsync(int? level)
    //{
    //    List<int> levels = new List<int>();
    //    var roles =
    //        await SugarRepository.QueryMuchAsync<Role, UserRole, Role>(
    //            (r, ur) => new object[]
    //            {
    //                JoinType.Left, r.Id == ur.RoleId
    //            },
    //            (r, ur) => r,
    //            (r, ur) => ur.UserId == App.HttpUser.Id
    //        );
    //    levels.AddRange(roles.Select(x => x.Level).ToList());
    //    int minLevel = levels.Min();
    //    if (level != null && level < minLevel)
    //    {
    //        throw new BusException("您无权修改或删除比你角色等级更高的数据！");
    //    }

    //    return minLevel;
    //}

    /// <summary>
    /// 查询角色等级
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int?> QueryUserRoleLevelAsync(HashSet<long> ids)
    {
        var levels = await SugarClient.Queryable<Role, UserRole>((r, ur) => new JoinQueryInfos(
                JoinType.Left, r.Id == ur.RoleId
            )).Where((r, ur) => ids.Contains(ur.UserId))
            .Select((r) => r.Level).ToListAsync();
        if (levels.Any())
        {
            var minLevel = levels.Min();
            return minLevel;
        }

        return null;
    }
    /// <summary>
    /// 验证用户角色等级
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public async Task<int> VerificationUserRoleLevelAsync(int? level)
    {
        var minLevel = 999;
        var levels = await SugarClient.Queryable<Role, UserRole>((r, ur) => new JoinQueryInfos(
                JoinType.Left, r.Id == ur.RoleId
            )).Where((r, ur) => ur.UserId == App.HttpUser.Id)
            .Select((r) => r.Level).ToListAsync();

        if (levels.Any())
        {
            minLevel = levels.Min();
        }

        if (level != null && level < minLevel)
        {
            throw new BadRequestException(
                App.L.R("Error.PermissionDenied.HigherRoleData"));
        }

        return minLevel;
    }


    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    public async Task<RolePermissionParam> GetPermissionAsync(long roleId)
    {
         var model = new RolePermissionParam() {RoleId=roleId  };
        var role = await GetIQueryable()
    .Includes(x => x.Menus)       // Load Menus navigation property
    .Includes(x => x.Apis)        // Load Apis navigation property
    .Where(x => x.Id == roleId)
    .Select(x => new {
        MenuIds = x.Menus.Select(m => m.Id).ToList(),
        Apis = x.Apis.Select(a => new { a.Id, a.Version }).ToList()
    })
    .FirstAsync();

        model.MenuIds = role.MenuIds;
        model.Pc= role.Apis.Where(x => x.Version == (int)VersionEnum.Pc).Select(x => x.Id).ToList();

        model.Def = role.Apis.Where(x => x.Version == (int)VersionEnum.Def).Select(x => x.Id).ToList();
        model.App = role.Apis.Where(x => x.Version == (int)VersionEnum.App).Select(x => x.Id).ToList();
        return model;
    }

    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task SetPermissionAsync(RolePermissionParam param)
    {
        var entity = new Role
        {
           Id=param.RoleId
            
        };
        entity.Menus = param.MenuIds.Select(x => new Menu { Id = x }).ToList();
        entity.Apis = new List<Apis>();
        entity.Apis.AddRange(param.Def?.Select(y => new Apis
        {
            Id = y
        }).ToList());
        entity.Apis.AddRange(param.Pc?.Select(y => new Apis
        {
            Id = y
        }).ToList());
        entity.Apis.AddRange(param.App?.Select(y => new Apis
        {
            Id = y
        }).ToList());


        SugarRepository.SugarClient.UpdateNav(entity)
            .Include(z1 => z1.Menus)
             .Include(z1 => z1.Apis)
            .ExecuteCommand();
    }
    #endregion



}
