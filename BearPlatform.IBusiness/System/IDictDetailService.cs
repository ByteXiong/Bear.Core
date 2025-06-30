using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.System.Dict;
using BearPlatform.Models.Dto.Core.System.Dict;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.ViewModel.Core.System.Dict;

namespace BearPlatform.IBusiness.System;

/// <summary>
/// 字典详情接口
/// </summary>
public interface IDictDetailService : IBaseServices<DictDetail>
{
    #region 基础接口

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="createUpdateDictDetailDto"></param>
    /// <returns></returns>
    Task<OperateResult> CreateAsync(CreateUpdateDictDetailDto createUpdateDictDetailDto);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="createUpdateDictDetailDto"></param>
    /// <returns></returns>
    Task<OperateResult> UpdateAsync(CreateUpdateDictDetailDto createUpdateDictDetailDto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(long id);


    /// <summary>
    /// 查询 根据字典ID
    /// </summary>
    /// <param name="dictId"></param>
    /// <returns></returns>
    Task<List<DictDetailVo>> GetDetailByDictIdAsync(long dictId);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="dictDetailQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<List<DictDetailVo>> QueryAsync(DictDetailQueryCriteria dictDetailQueryCriteria, Pagination pagination);

    #endregion
}
