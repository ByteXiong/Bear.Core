using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Models.Dto.Core.Permission;
using BearPlatform.Models.Dto.Permission;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Permission;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Permission.Dept;

namespace BearPlatform.IBusiness.Permission;

/// <summary>
///  部门
/// </summary>
public interface IDeptService : IBaseServices<Dept>
{

    #region 基础接口

    /// <summary>
    ///  分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<DeptDTO>> GetPageAsync(DeptParam param);
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<DeptInfo> GetInfoAsync(long id);
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateDeptParam param);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> UpdateAsync(UpdateDeptParam param);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(HashSet<long> ids);

    #endregion

    #region 扩展接口

    
    #endregion

}
