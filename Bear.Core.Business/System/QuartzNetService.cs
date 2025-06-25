using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bear.Core.Common.Attributes;
using Bear.Core.Common.Exception;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Global;
using Bear.Core.Common.Model;
using Bear.Core.Core;
using Bear.Core.Core.Utils;
using Bear.Core.Entity.Core.System.QuartzNet;
using Bear.Core.IBusiness.System;
using Bear.Core.Models;
using Bear.Core.Models.Dto.Core.System;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.System;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel.Core.System.QuartzNet;
using Bear.Core.ViewModel.Report.System;

namespace Bear.Core.Business.System;
/// <summary>
/// QuartzNet作业服务
/// </summary>
public class QuartzNetService : BaseServices<QuartzNet>, IQuartzNetService
{


    #region 字段
    readonly IQuartzNetLogService _quartzNetLogService;
    #endregion
    #region 构造函数
    public QuartzNetService(IQuartzNetLogService quartzNetLogService) : base()
    {
        _quartzNetLogService = quartzNetLogService;
    }
    #endregion
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<QuartzNetDTO>> GetPageAsync(QuartzNetParam param)
    {

        var page = await GetIQueryable().Select(x => new QuartzNetDTO
        {
        }, true).SearchWhere(param).ToPagedResultsAsync(param);
        return page;
    }

    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<QuartzNetInfo> GetInfoAsync(long id)
    {
        var entity = await GetIQueryable(x => x.Id == id).Select<QuartzNetInfo>().FirstAsync();
        return entity;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(UpdateQuartzNetParam param)
    {
        if (await GetIQueryable(q =>
                 q.AssemblyName == param.AssemblyName &&
                 q.ClassName == param.ClassName).AnyAsync())
        {
            throw new BadRequestException(
                ValidationError.IsExist(param, nameof(param.ClassName)));
        }

        var model = App.Mapper.MapTo<QuartzNet>(param);
        await SugarClient.Insertable(model).ExecuteReturnEntityAsync();
        return model.Id;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<long> UpdateAsync(UpdateQuartzNetParam param)
    {

        var oldQuartzNet =
            await TableWhere(x => x.Id == param.Id).FirstAsync();
        if (oldQuartzNet.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param,
                LanguageKeyConstants.QuartzNet,
                nameof(param.Id)));
        }

        if ((oldQuartzNet.AssemblyName != param.AssemblyName ||
             oldQuartzNet.ClassName != param.ClassName) && await TableWhere(q =>
                q.AssemblyName == param.AssemblyName &&
                q.ClassName == param.ClassName).AnyAsync())
        {
            throw new BusException(
                ValidationError.IsExist(param, nameof(param.ClassName)));
        }

        var model = App.Mapper.MapTo<QuartzNet>(param);
        await UpdateAsync(model);
        return model.Id;
    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(HashSet<long> ids)
    {
        return await LogicDeleteAsync<QuartzNet>(x => ids.Contains(x.Id));
    }
    #endregion


    /// <summary>
    /// 查询全部
    /// </summary>
    /// <returns></returns>
    public async Task<List<QuartzNet>> QueryAllAsync()
    {
        return await Table.ToListAsync();
    }
    /// <summary>
    /// 更新作业
    /// </summary>
    /// <param name="quartzNet"></param>
    /// <param name="quartzNetLog"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<OperateResult> UpdateJobInfoAsync(QuartzNet quartzNet, QuartzNetLog quartzNetLog)
    {
        await UpdateAsync(quartzNet);
        await _quartzNetLogService.AddAsync(quartzNetLog);
        return OperateResult.Success();
    }
}
