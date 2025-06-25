using System.Collections.Generic;
using System.Threading.Tasks;
using Bear.Core.Common.Model;
using Bear.Core.Entity.Core.System.QuartzNet;
using Bear.Core.Models;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.System;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel.Core.System.QuartzNet;

namespace Bear.Core.IBusiness.System;
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
