using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Global;
using Bear.Core.Common.Model;
using Bear.Core.Common.WebApp;
using Bear.Core.Core;
using Bear.Core.Core.Caches;
using Bear.Core.Entity.Core.System;
using Bear.Core.IBusiness.Monitor;
using Bear.Core.IBusiness.System;
using Bear.Core.Models;
using Bear.Core.Models.Queries.Common;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel.Report.Monitor;

namespace Bear.Core.Business.Monitor;
/// <summary>
/// 在线用户服务
/// </summary>
public class OnlineUserService : IOnlineUserService
{


    #region 字段
    private readonly ICache _cache;
    readonly ITokenBlacklistService _tokenBlacklistService;
    #endregion
    #region 构造函数
    public OnlineUserService(ICache cache, ITokenBlacklistService tokenBlacklistService)
    {
        _cache = cache;
        _tokenBlacklistService = tokenBlacklistService;
    }
    #endregion
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<LoginUserInfo>> GetPageAsync(OnlineUserParam param)
    {
        List<LoginUserInfo> loginUserInfos = new List<LoginUserInfo>();
        var arrayList = await _cache.ScriptEvaluateKeys(GlobalConstants.CachePrefix.OnlineKey);
        if (arrayList.Length > 0)
        {
            foreach (var item in arrayList)
            {
                var loginUserInfo =
                    await _cache.GetAsync<LoginUserInfo>(item);
                if (loginUserInfo.IsNull()) continue;
                loginUserInfo.AccessToken = loginUserInfo.AccessToken.ToMd5String16();
                loginUserInfos.Add(loginUserInfo);
            }
        }

        List<LoginUserInfo> newOnlineUsers = new List<LoginUserInfo>();
        if (loginUserInfos.Count > 0)
        {
            newOnlineUsers = loginUserInfos.Skip((param.PageIndex - 1) * param.PageSize)
                .Take(param.PageSize)
                .ToList();
        }

        return  new PagedResults<LoginUserInfo> { 
         Data = newOnlineUsers,
         PagerInfo= new PagerInfo( param) { 
              
         }
         
        };
    }
    #endregion
    #region 扩展接口

    /// <summary>
    /// 强退
    /// </summary>
    /// <param name="ids"></param>
    public async Task DropOutAsync(HashSet<string> ids)
    {
        var list = new List<TokenBlacklist>();
        list.AddRange(ids.Select(x => new TokenBlacklist { AccessToken = x }));
        if (await _tokenBlacklistService.AddAsync(list) > 0)
        {
            foreach (var item in ids)
            {
                await _cache.RemoveAsync(GlobalConstants.CachePrefix.OnlineKey + item);
            }
        }
    }
    #endregion
}


