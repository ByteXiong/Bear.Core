using System.ComponentModel.DataAnnotations;

namespace BearPlatform.Common.Enums;

/// <summary>
/// 租户类型
/// </summary>
public enum TenantType
{
    /// <summary>
    /// Id隔离
    /// </summary>
    [Display(Name = "Enum.Tenant.Id")]
    Id = 1,

    /// <summary>
    /// 库隔离
    /// </summary>
    [Display(Name = "Enum.Tenant.Db")]
    Db = 2
}
