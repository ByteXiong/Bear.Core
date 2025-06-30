using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.System.QuartzNet;
using BearPlatform.Models;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.System.QuartzNet;

namespace BearPlatform.IBusiness.System;
/// <summary>
///  QuartzNet作业日志服务
/// </summary>
public interface IQuartzNetLogService : IBaseServices<QuartzNetLog>
{
    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<QuartzNetLogDTO>> GetPageAsync(QuartzNetLogParam param);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateQuartzNetLogParam param);

 

    #endregion

    #region 扩展接口

    #endregion

}
