using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.System.QuartzNet;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.System;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.System.QuartzNet;

namespace BearPlatform.IBusiness.System;

/// <summary>
///  QuartzJob作业接口
/// </summary>
public interface IQuartzNetService : IBaseServices<QuartzNet>
{

    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<QuartzNetDTO>> GetPageAsync(QuartzNetParam param);
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<QuartzNetInfo> GetInfoAsync(long id);
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateQuartzNetParam param);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> UpdateAsync(UpdateQuartzNetParam param);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(HashSet<long> ids);

    #endregion

    #region 扩展接口
    /// <summary>
    /// 查询全部
    /// </summary>
    /// <returns></returns>
    Task<List<QuartzNet>> QueryAllAsync();
    /// <summary>
    /// 更新任务与日志
    /// </summary>
    /// <param name="quartzNet"></param>
    /// <param name="quartzNetLog"></param>
    /// <returns></returns>
    Task<OperateResult> UpdateJobInfoAsync(QuartzNet quartzNet, QuartzNetLog quartzNetLog);
    #endregion

}
