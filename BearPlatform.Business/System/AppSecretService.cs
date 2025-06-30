using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.IdGenerator;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.Core.Utils;
using BearPlatform.Entity.Core.System;
using BearPlatform.IBusiness.System;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.System;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.System;
using BearPlatform.ViewModel.Report.System;

namespace BearPlatform.Business.System;

/// <summary>
/// App应用秘钥
/// </summary>
public class AppSecretService : BaseServices<AppSecret>, IAppSecretService
{


    #region 字段

    #endregion
    #region 构造函数
    public AppSecretService() : base()
    {
    }
    #endregion
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<AppSecretDTO>> GetPageAsync(AppSecretParam param)
    {

        var page = await GetIQueryable().Select(x => new AppSecretDTO
        {
        }, true).SearchWhere(param).ToPagedResultsAsync(param);
        return page;
    }

    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<AppSecretInfo> GetInfoAsync(long id)
    {
        var entity = await GetIQueryable(x => x.Id == id).Select<AppSecretInfo>().FirstAsync();
        return entity;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(UpdateAppSecretParam param)
    {
        if (await TableWhere(r => r.AppName == param.AppName).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.AppName)));
        }

        var id = IdHelper.NextId().ToString();
        param.AppId = DateTime.Now.ToString("yyyyMMdd") + id[..8];
        param.AppSecretKey =
            (param.AppId + id).ToHmacsha256String(App.GetOptions<SystemOptions>().HmacSecret);
        var model = App.Mapper.MapTo<AppSecret>(param);
         await AddAsync(model);
        return model.Id;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<long> UpdateAsync(UpdateAppSecretParam param)
    {

        //取出待更新数据
        var oldAppSecret = await TableWhere(x => x.Id == param.Id).FirstAsync();
        if (oldAppSecret.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param,
                LanguageKeyConstants.AppSecret,
                nameof(param.Id)));
        }

        if (oldAppSecret.AppName != param.AppName &&
            await TableWhere(x => x.AppName == param.AppName).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.AppName)));
        }

        var model = App.Mapper.MapTo<AppSecret>(param);
        var result = await UpdateAsync(model, null, x => new
        {
            x.AppId,
            x.AppSecretKey
        });
        return model.Id;
    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(HashSet<long> ids)
    {
        var appSecrets = await GetIQueryable(x => ids.Contains(x.Id)).ToListAsync();
        if (appSecrets.Count <= 0)
        {
            throw new BusException(ValidationError.NotExist());
        }

        return await LogicDeleteAsync<AppSecret>(x => ids.Contains(x.Id));
       
    }
    #endregion

}

