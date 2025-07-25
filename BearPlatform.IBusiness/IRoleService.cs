using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Entity.Core.Permission.Role;
using BearPlatform.Models.Permission;
using BearPlatform.SqlSugar;

namespace BearPlatform.IBusiness.Permission;

/// <summary>
/// 角色接口
/// </summary>
public interface IRoleService : IBaseServices<Role>
{
    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<RoleDTO>> GetPageAsync(RoleParam param);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="createUpdateRoleDto"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateRoleParam createUpdateRoleDto);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="createUpdateRoleDto"></param>
    /// <returns></returns>
    Task<long> UpdateAsync(UpdateRoleParam createUpdateRoleDto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(HashSet<long> ids);

    #endregion

    ///// <summary>
    ///// 查询
    ///// </summary>
    ///// <param name="roleQueryCriteria"></param>
    ///// <param name="pagination"></param>
    ///// <returns></returns>
    //Task<List<RoleDto>> QueryAsync(RoleQueryCriteria roleQueryCriteria, Pagination pagination);

    ///// <summary>
    ///// 下载
    ///// </summary>
    ///// <param name="roleQueryCriteria"></param>
    ///// <returns></returns>
    //Task<List<ExportBase>> DownloadAsync(RoleQueryCriteria roleQueryCriteria);

    //#endregion

    //#region 扩展接口

    ///// <summary>
    ///// 获取全部角色
    ///// </summary>
    ///// <returns></returns>
    //Task<List<RoleDto>> QueryAllAsync();

    ///// <summary>
    ///// 获取用户角色等级
    ///// </summary>
    ///// <param name="ids"></param>
    ///// <returns></returns>
    //Task<int> QueryUserRoleLevelAsync(HashSet<long> ids);

    ///// <summary>
    ///// 验证角色等级
    ///// </summary>
    ///// <param name="level"></param>
    ///// <returns></returns>
    //Task<int> VerificationUserRoleLevelAsync(int? level);

    ///// <summary>
    ///// 更新角色菜单
    ///// </summary>
    ///// <param name="createUpdateRoleDto"></param>
    ///// <returns></returns>
    //Task<bool> UpdateRolesMenusAsync(CreateUpdateRoleDto createUpdateRoleDto);

    ///// <summary>
    ///// 更新角色Apis
    ///// </summary>
    ///// <param name="createUpdateRoleDto"></param>
    ///// <returns></returns>
    //Task<bool> UpdateRolesApisAsync(CreateUpdateRoleDto createUpdateRoleDto);

    //#endregion

    #region 扩展接口

    /// <summary>
    /// 获取用户角色等级
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int?> QueryUserRoleLevelAsync(HashSet<long> ids);

    /// <summary>
    /// 验证角色等级
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    Task<int> VerificationUserRoleLevelAsync(int? level);
    /// <summary>
    /// 获取权限
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    Task<RolePermissionParam> GetPermissionAsync(long roleId);
    /// <summary>
    /// 设置权限
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task SetPermissionAsync(RolePermissionParam param);
    #endregion



}
