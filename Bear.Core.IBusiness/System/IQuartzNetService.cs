using System.Collections.Generic;
using System.Threading.Tasks;
using Bear.Core.Common.Model;
using Bear.Core.Entity.Core.System.QuartzNet;
using Bear.Core.Models;
using Bear.Core.Models.Dto.Core.System;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.System;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel.Core.System.QuartzNet;

namespace Bear.Core.IBusiness.System;

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
