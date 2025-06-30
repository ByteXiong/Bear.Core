using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.Utils;
using BearPlatform.Entity;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.IBusiness.Permission;
using BearPlatform.Models.Dto.Core.Permission;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Permission;
using BearPlatform.ViewModel.Core.Permission;

namespace BearPlatform.Business.Permission;

/// <summary>
/// Api路由服务
/// </summary>
public class ApisService : BaseServices<Apis>, IApisService
{
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="createUpdateApisDto"></param>
    /// <returns></returns>
    public async Task<OperateResult> CreateAsync(CreateUpdateApisDto createUpdateApisDto)
    {
        if (await TableWhere(a => a.Url == createUpdateApisDto.Url).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(createUpdateApisDto, nameof(createUpdateApisDto.Url)));
        }

        var apis = App.Mapper.MapTo<Apis>(createUpdateApisDto);
        var result = await AddAsync(apis);
        return OperateResult.Result(result);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="createUpdateApisDto"></param>
    /// <returns></returns>
    public async Task<OperateResult> UpdateAsync(CreateUpdateApisDto createUpdateApisDto)
    {
        var oldApis =
            await TableWhere(x => x.Id == createUpdateApisDto.Id).FirstAsync();
        if (oldApis.IsNull())
        {
            throw new BusException(ValidationError.NotExist(createUpdateApisDto, LanguageKeyConstants.Api,
                nameof(createUpdateApisDto.Id)));
        }

        if (oldApis.Url != createUpdateApisDto.Url &&
            await TableWhere(a => a.Url == createUpdateApisDto.Url).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(createUpdateApisDto, nameof(createUpdateApisDto.Url)));
        }

        var apis = App.Mapper.MapTo<Apis>(createUpdateApisDto);
        var result = await UpdateAsync(apis);
        return OperateResult.Result(result);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(HashSet<Guid> ids)
    {
        var apis = await TableWhere(x => ids.Contains(x.Id)).ToListAsync();
        if (apis.Count < 1)
        {
            throw new BusException(ValidationError.NotExist());
        }

        var result = await LogicDelete<Apis>(x => ids.Contains(x.Id));
        return result;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="apisQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public async Task<List<ApisVo>> QueryAsync(ApisQueryCriteria apisQueryCriteria, Pagination pagination)
    {
        var queryOptions = new QueryOptions<Apis>
        {
            Pagination = pagination,
            ConditionalModels = apisQueryCriteria.ApplyQueryConditionalModel()
        };
        return App.Mapper.MapTo<List<ApisVo>>(await TablePageAsync(queryOptions));
    }

    /// <summary>
    /// 查询全部
    /// </summary>
    /// <returns></returns>
    public async Task<List<ApisVo>> QueryAllAsync()
    {
        return App.Mapper.MapTo<List<ApisVo>>(await Table.ToListAsync());
    }

    /// <summary>
    /// 批量创建
    /// </summary>
    /// <param name="apis"></param>
    /// <returns></returns>
    public async Task<OperateResult> CreateAsync(List<Apis> apis)
    {
        var result = await AddAsync(apis);
        return OperateResult.Result(result);
    }
}
