using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
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

namespace BearPlatform.Business.System;

/// <summary>
///  租户
/// </summary>
public class TenantService : BaseServices<Tenant>, ITenantService
{


    #region 字段

    #endregion
    #region 构造函数
    public TenantService() : base()
    {
    }
    #endregion
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<TenantDTO>> GetPageAsync(TenantParam param)
    {

        var page = await GetIQueryable().Select(x => new TenantDTO
        {
        }, true).SearchWhere(param).ToPagedResultsAsync(param);
        return page;
    }

    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<TenantInfo> GetInfoAsync(long id)
    {
        var entity = await GetIQueryable(x => x.Id == id).Select<TenantInfo>().FirstAsync();
        return entity;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> AddAsync(UpdateTenantParam param)
    {
        if (await TableWhere(r => r.TenantId == param.TenantId).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.TenantId)));
        }

        if (param.TenantType == TenantType.Db)
        {
            if (param.DbType.IsNull())
            {
                throw new BusException(App.L.R("{0}required",
                    App.L.R("Tenant.DbType")));
            }

            if (param.ConfigId.IsNullOrEmpty())
            {
                throw new BusException(App.L.R("{0}required",
                    App.L.R("Tenant.ConfigId")));
            }

            if (param.ConnectionString.IsNullOrEmpty())
            {
                throw new BusException(App.L.R("{0}required",
                    App.L.R("Tenant.Connection")));
            }

            if (await TableWhere(r => r.ConfigId == param.ConfigId).AnyAsync())
            {
                throw new BusException(ValidationError.IsExist(param,
                    nameof(param.ConfigId)));
            }
        }

        var model = App.Mapper.MapTo<Tenant>(param);
        var result = await AddAsync(model);
        return model.Id;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> UpdateAsync(UpdateTenantParam param)
    {

        //取出待更新数据
        var oldTenant = await TableWhere(x => x.Id == param.Id).FirstAsync();
        if (oldTenant.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param, LanguageKeyConstants.Tenant,
                nameof(param.Id)));
        }

        if (oldTenant.TenantId != param.TenantId &&
            await TableWhere(x => x.TenantId == param.TenantId).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.TenantId)));
        }

        if (param.TenantType == TenantType.Db)
        {
            if (param.DbType.IsNull())
            {
                throw new BusException(App.L.R("{0}required",
                    App.L.R("Tenant.DbType")));
            }

            if (param.ConfigId.IsNullOrEmpty())
            {
                throw new BusException(App.L.R("{0}required",
                    App.L.R("Tenant.ConfigId")));
            }

            if (param.ConnectionString.IsNullOrEmpty())
            {
                throw new BusException(App.L.R("{0}required",
                    App.L.R("Tenant.Connection")));
            }

            if (oldTenant.ConfigId != param.ConfigId &&
                await TableWhere(x => x.ConfigId == param.ConfigId).AnyAsync())
            {
                throw new BusException(ValidationError.IsExist(param,
                    nameof(param.ConfigId)));
            }
        }

        var model = App.Mapper.MapTo<Tenant>(param);
        var result = await UpdateAsync(model, null, x => x.TenantId);
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

        var tenants = await TableWhere(x => ids.Contains(x.Id)).Includes(x => x.Users).ToListAsync();
        if (tenants.Any(x => x.Users != null && x.Users.Count != 0))
        {
            throw new BusException(ValidationError.DataAssociationExists());
        }
        return await LogicDeleteAsync<Tenant>(x => ids.Contains(x.Id));
    }
    #endregion
    #region 扩展接口
    #endregion
}
