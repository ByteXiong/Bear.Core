using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Model;
using BearPlatform.Core;
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
using Microsoft.Extensions.Logging;
using static BearPlatform.Common.Helper.ExceptionHelper;

namespace BearPlatform.Business.System;

/// <summary>
/// 全局设置服务
/// </summary>
public class SettingService : BaseServices<Setting>, ISettingService
{
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<SettingDTO>> GetPageAsync(SettingParam param)
    {
        var page = await GetIQueryable().Select(x => new SettingDTO
        {
        }, true).SearchWhere(param).ToPagedResultsAsync(param);
        return page;
    }
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<SettingInfo> GetInfoAsync(long id)
    {
        var model = await GetIQueryable(x => x.Id == id).Select<SettingInfo>().FirstAsync();
        return model;
    }


    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(UpdateSettingParam param)
    {
        if (await TableWhere(r => r.Name == param.Name).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Name)));
        }
        var model = App.Mapper.MapTo<Setting>(param);
        await AddAsync(model);
        return model.Id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<long> UpdateAsync(UpdateSettingParam param)
    {
        //取出待更新数据
        var oldSetting = await TableWhere(x => x.Id == param.Id).FirstAsync();
        if (oldSetting.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param, LanguageKeyConstants.Setting,
                nameof(param.Id)));
        }

        if (oldSetting.Name != param.Name &&
            await TableWhere(x => x.Name == param.Name).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Name)));
        }

        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.LoadSettingByName +
                                    oldSetting.Name.ToMd5String16());
        var model = App.Mapper.MapTo<Setting>(param);
        var result = await UpdateAsync(model);

        return model.Id;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(HashSet<long> ids)
    {
        var settings = await TableWhere(x => ids.Contains(x.Id)).ToListAsync();
        if (settings.Count == 0)
        {
            throw new BusException(ValidationError.NotExist());
        }

        foreach (var setting in settings)
        {
            await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.LoadSettingByName +
                                        setting.Name.ToMd5String16());
        }

      return await LogicDeleteAsync<Setting>(x => ids.Contains(x.Id));

    }

 

  

    /// <summary>
    /// 获取设置 值
    /// </summary>
    /// <param name="settingName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [UseCache(Expiration = 30, KeyPrefix = GlobalConstants.CachePrefix.LoadSettingByName)]
    public async Task<T> GetSettingValue<T>(string settingName)
    {
        var setting = await TableWhere(x => x.Name == settingName).FirstAsync();

        if (setting == null) return default;

        try
        {
            return (T)ConvertValue(typeof(T), setting.Value);
        }
        catch (Exception e)
        {
            App.GetService<ILogger<Setting>>().LogError(GetExceptionAllMsg(e));
            return default;
        }
    }

    /// <summary>
    /// 类型转换
    /// </summary>
    /// <param name="type"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static object ConvertValue(Type type, string value)
    {
        if (type == typeof(object))
        {
            return value;
        }

        if (type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
        {
            return string.IsNullOrEmpty(value) ? value : ConvertValue(Nullable.GetUnderlyingType(type), value);
        }

        var converter = TypeDescriptor.GetConverter(type);
        return converter.CanConvertFrom(typeof(string)) ? converter.ConvertFromInvariantString(value) : null;
    }

    #endregion
}
