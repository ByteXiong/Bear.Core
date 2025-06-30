using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Common.WebApp;
using BearPlatform.Models;
using BearPlatform.Models.Queries.Common;
using BearPlatform.SqlSugar;

namespace BearPlatform.IBusiness.Monitor;

/// <summary>
/// 在线用户接口
/// </summary>
public interface IOnlineUserService
{
    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<LoginUserInfo>> GetPageAsync(OnlineUserParam param);





    #endregion

    /// <summary>
    /// 强退
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task DropOutAsync(HashSet<string> ids);
}
