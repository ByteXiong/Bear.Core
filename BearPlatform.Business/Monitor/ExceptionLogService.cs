using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Entity.Logs;
using BearPlatform.IBusiness.Monitor;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel;
using BearPlatform.ViewModel.Core.Monitor;

namespace BearPlatform.Business.Monitor;

/// <summary>
/// 系统日志
/// </summary>
public class ExceptionLogService : BaseServices<ExceptionLog>, IExceptionLogService
{


    #region 字段

    #endregion
    #region 构造函数
    public ExceptionLogService() : base()
    {
    }
    #endregion
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<ExceptionLogDTO>> GetPageAsync(ExceptionLogParam param)
    {

        var page = await GetIQueryable().Select(x => new ExceptionLogDTO
        {
        }, true).SearchWhere(param).ToPagedResultsAsync(param);
        return page;
    }
    #endregion
    #region 扩展接口
    #endregion
}


