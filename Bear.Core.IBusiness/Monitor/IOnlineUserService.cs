using System.Collections.Generic;
using System.Threading.Tasks;
using Bear.Core.Common.Model;
using Bear.Core.Common.WebApp;
using Bear.Core.Models;
using Bear.Core.Models.Queries.Common;
using Bear.Core.SqlSugar;

namespace Bear.Core.IBusiness.Monitor;

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
