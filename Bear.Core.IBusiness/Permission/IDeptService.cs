using System.Collections.Generic;
using System.Threading.Tasks;
using Bear.Core.Common.Model;
using Bear.Core.Entity.Core.Permission;
using Bear.Core.Models.Dto.Core.Permission;
using Bear.Core.Models.Dto.Permission;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.Permission;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel.Core.Permission.Dept;

namespace Bear.Core.IBusiness.Permission;

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
