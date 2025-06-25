using System.Collections.Generic;
using System.Threading.Tasks;
using Bear.Core.Common.Attributes;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Model;
using Bear.Core.Core;
using Bear.Core.Entity.Logs;
using Bear.Core.IBusiness.Monitor;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.System;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel;
using Bear.Core.ViewModel.Core.Monitor;

namespace Bear.Core.Business.Monitor;

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


