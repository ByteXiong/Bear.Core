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
/// 字典接口
/// </summary>
public interface IDictService : IBaseServices<Dict>
{
    #region 基础接口

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="createUpdateDictDto"></param>
    /// <returns></returns>
    Task<OperateResult> CreateAsync(CreateUpdateDictDto createUpdateDictDto);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="createUpdateDictDto"></param>
    /// <returns></returns>
    Task<OperateResult> UpdateAsync(CreateUpdateDictDto createUpdateDictDto);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(HashSet<long> ids);

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="dictQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<List<DictVo>> QueryAsync(DictQueryCriteria dictQueryCriteria, Pagination pagination);

    /// <summary>
    /// 下载
    /// </summary>
    /// <param name="dictQueryCriteria"></param>
    /// <returns></returns>
    Task<List<ExportBase>> DownloadAsync(DictQueryCriteria dictQueryCriteria);

    #endregion
}
