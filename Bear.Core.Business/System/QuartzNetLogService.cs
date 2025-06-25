using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Model;
using Bear.Core.Core;
using Bear.Core.Entity.Core.System.QuartzNet;
using Bear.Core.IBusiness.System;
using Bear.Core.Models;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.System;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel.Core.System.QuartzNet;
using Bear.Core.ViewModel.Report.System;

namespace Bear.Core.Business.System;

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

