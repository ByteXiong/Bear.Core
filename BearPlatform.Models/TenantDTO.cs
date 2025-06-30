using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.System;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;

namespace BearPlatform.Models
{

    /// <summary>
    /// 租户
    /// </summary>
    #region 查询参数
    public class TenantParam : PageParam
    {

    }
    #endregion

    #region DTO
    /// <summary>
    ///  分页
    /// </summary>
    [AutoMapping(typeof(TenantDTO), typeof(Tenant))]
    public class TenantDTO : RootKeyDTO<Guid>
    {


    }

    /// <summary>
    /// 详情
    /// </summary>
    [AutoMapping(typeof(TenantInfo), typeof(Tenant))]
    public class TenantInfo : Tenant
    {

    }
    /// <summary>
    /// 更新
    /// </summary>
    [AutoMapping(typeof(UpdateTenantParam), typeof(Tenant))]
    public class UpdateTenantParam : Tenant
    {
    }
    #endregion


}
