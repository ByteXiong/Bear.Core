using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.System;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.System;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.System;

namespace BearPlatform.IBusiness.System;

/// <summary>
///  应用秘钥
/// </summary>
public interface IAppSecretService : IBaseServices<AppSecret>
{

    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<AppSecretDTO>> GetPageAsync(AppSecretParam param);

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<AppSecretInfo> GetInfoAsync(long id);
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateAppSecretParam param);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> UpdateAsync(UpdateAppSecretParam param);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(HashSet<long> ids);

    #endregion

    #region 扩展接口

    #endregion

}
