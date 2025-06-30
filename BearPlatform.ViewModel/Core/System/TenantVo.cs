using System.Data;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Entity.Core.System;
using BearPlatform.Models.Base;

namespace BearPlatform.ViewModel.Core.System;

/// <summary>
/// 租户Vo
/// </summary>
[AutoMapping(typeof(Tenant), typeof(TenantVo))]
public class TenantVo : BaseEntityDTO<long>
{
    /// <summary>
    /// 租户Id
    /// </summary>
    public int TenantId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    public TenantType TenantType { get; set; }

    /// <summary>
    /// 库Id
    /// </summary>
    public string ConfigId { get; set; }

    /// <summary>
    /// 库类型
    /// </summary>
    public DbType DbType { get; set; }

    /// <summary>
    /// 库连接
    /// </summary>
    public string ConnectionString { get; set; }
}
