using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Permission;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Permission;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Permission.Job;

namespace BearPlatform.IBusiness.Permission;

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
