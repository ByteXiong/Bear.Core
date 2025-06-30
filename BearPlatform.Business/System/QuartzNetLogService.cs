using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Entity.Core.System.QuartzNet;
using BearPlatform.IBusiness.System;
using BearPlatform.Models;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.System.QuartzNet;
using BearPlatform.ViewModel.Report.System;

namespace BearPlatform.Business.System;

/// <summary>
/// QuartzNet作业日志服务
/// </summary>
public class QuartzNetLogService : BaseServices<QuartzNetLog>, IQuartzNetLogService
{


    #region 字段

    #endregion
    #region 构造函数
    public QuartzNetLogService() : base()
    {
    }
    #endregion
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<QuartzNetLogDTO>> GetPageAsync(QuartzNetLogParam param)
    {

        var page = await GetIQueryable().Select(x => new QuartzNetLogDTO
        {
        }, true).SearchWhere(param).ToPagedResultsAsync(param);
        return page;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(UpdateQuartzNetLogParam param)
    {
        var model = App.Mapper.MapTo<QuartzNetLog>(param);
        await AddAsync(model);
        return model.Id;
    }

  


    #endregion

}

