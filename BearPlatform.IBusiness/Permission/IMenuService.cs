using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Enums;
using BearPlatform.Entity.Permission;
using BearPlatform.Models.Permission;

namespace BearPlatform.IBusiness.Permission;

/// <summary>
/// 菜单接口
/// </summary>
public interface IMenuService : IBaseServices<Menu>
{
    #region 基础接口

  
    #endregion

    #region 扩展接口

    ///// <summary>
    ///// 获取所有菜单
    ///// </summary>
    ///// <returns></returns>
    //Task<List<RouteDTO>> QueryAllAsync();

    ///// <summary>
    ///// 根据父ID获取菜单
    ///// </summary>
    ///// <param name="pid"></param>
    ///// <returns></returns>
    //Task<List<RouteDTO>> FindByPIdAsync(long pid = 0);


    /// <summary>
    /// 构建前端菜单树
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    Task<List<RouteDTO>> BuildTreeAsync(long userId);


    /// <summary>
    /// 常量路由
    /// </summary>
    /// <returns></returns>
    Task<List<RouteDTO>> ConstantRoutesAsync();

    /// <summary>
    /// 判断路由是否存在
    /// </summary>
    /// <returns></returns>
    Task<bool> IsRouteExistAsync(string name);

    /// <summary>
    /// 列表
    /// </summary>
    /// <returns></returns>
    Task<List<MenuTreeDTO>> GetTreeAsync();


    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>

    Task<MenuInfo> GetInfoAsync(long id);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>

    Task<long> AddAsync(UpdateMenuParam param);
    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>

    Task<long> UpdateAsync(UpdateMenuParam param);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(long[] ids);

    /// <summary>
    /// 菜单下拉
    /// </summary>
    /// <returns></returns>
    Task<List<RouteTreeSelectDTO>> TreeSelectAsync(MenuTypeEnum[] types);
    ///// <summary>
    ///// 获取所有同级或父级菜单
    ///// </summary>
    ///// <param name="id"></param>
    ///// <returns></returns>
    //Task<List<RouteDTO>> FindSuperiorAsync(long id);

    ///// <summary>
    ///// 获取所有子菜单ID
    ///// </summary>
    ///// <param name="id"></param>
    ///// <returns></returns>
    //Task<List<long>> FindChildAsync(long id);




    #endregion
}
