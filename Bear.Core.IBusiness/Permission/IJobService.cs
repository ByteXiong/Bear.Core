using System.Collections.Generic;
using System.Threading.Tasks;
using Bear.Core.Common.Model;
using Bear.Core.Entity.Core.Permission;
using Bear.Core.Models;
using Bear.Core.Models.Dto.Core.Permission;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.Permission;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel.Core.Permission.Job;

namespace Bear.Core.IBusiness.Permission;

/// <summary>
///  岗位
/// </summary>
public interface IJobService : IBaseServices<Job>
{

    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<JobDTO>> GetPageAsync(JobParam param);
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<JobInfo> GetInfoAsync(long id);
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateJobParam param);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> UpdateAsync(UpdateJobParam param);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(HashSet<long> ids);

    #endregion

    #region 扩展接口
    /// <summary>
    /// 查询Select
    /// </summary>
    /// <returns></returns>
    Task<List<JobSelectDTO>> GetSelectAsync();
    #endregion

}
