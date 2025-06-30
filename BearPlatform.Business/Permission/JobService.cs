using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.Utils;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.IBusiness.Permission;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Permission;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Permission;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Permission.Job;
using BearPlatform.ViewModel.Report.Permission;
using SqlSugar;

namespace BearPlatform.Business.Permission;

/// <summary>
/// 岗位
/// </summary>
public class JobService : BaseServices<Job>, IJobService
{


    #region 字段

    #endregion
    #region 构造函数
    public JobService() : base()
    {
    }
    #endregion
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<JobDTO>> GetPageAsync(JobParam param)
    {

        var page = await GetIQueryable().Select<JobDTO>().SearchWhere(param).ToPagedResultsAsync(param);
        return page;
    }

    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<JobInfo> GetInfoAsync(long id)
    {
        var entity = await GetIQueryable(x => x.Id == id).Select<JobInfo>().FirstAsync();
        return entity;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> AddAsync(UpdateJobParam param)
    {
        if (await TableWhere(j => j.Name == param.Name).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Name)));
        }

        var model = App.Mapper.MapTo<Job>(param);
        var result = await AddAsync(model);
        return model.Id;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> UpdateAsync(UpdateJobParam param)
    {


        var oldJob =
           await TableWhere(x => x.Id == param.Id).FirstAsync();
        if (oldJob.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param, LanguageKeyConstants.Job,
                nameof(param.Id)));
        }

        if (oldJob.Name != param.Name &&
            await TableWhere(j => j.Name == param.Name).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Name)));
        }

        var model = App.Mapper.MapTo<Job>(param);
        await UpdateAsync(model);
        return model.Id;
    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<int> DeleteAsync(HashSet<long> ids)
    {

        var jobs = await TableWhere(x => ids.Contains(x.Id)).Includes(x => x.Users).ToListAsync();
        if (jobs.Count < 1)
        {
            throw new BusException(ValidationError.NotExist());
        }

        if (jobs.Any(job => job.Users != null && job.Users.Count != 0))
        {
            throw new BusException(ValidationError.DataAssociationExists());
        }
        return await LogicDeleteAsync<Job>(x => ids.Contains(x.Id));
    }
    #endregion
    #region 扩展接口

    /// <summary>
    /// 查询Select
    /// </summary>
    /// <returns></returns>
    public async Task<List<JobSelectDTO>> GetSelectAsync()
    {
        Expression<Func<Job, bool>> whereExpression = x => x.Enabled;
        var list=  await GetIQueryable(whereExpression).Select(x => new JobSelectDTO { })
            .ToListAsync();
        return list;
    }
    #endregion
}


