using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Logs;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel;
using BearPlatform.ViewModel.Core.Monitor;

namespace BearPlatform.IBusiness.Monitor;

/// <summary>
/// 系统日志接口
/// </summary>
public interface IExceptionLogService : IBaseServices<ExceptionLog>
{
    #region 基础接口
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<ExceptionLogDTO>> GetPageAsync(ExceptionLogParam param);

    #endregion
}
