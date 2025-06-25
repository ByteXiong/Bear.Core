using Bear.Core.Common.Attributes;
using Bear.Core.Common.Enums;

namespace Bear.Core.ViewModel.Core.Permission.Role;

/// <summary>
/// 角色Vo
/// </summary>
[AutoMapping(typeof(Entity.Core.Permission.Role.Role), typeof(RoleSmallVo))]
public class RoleSmallVo
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 权限标识符
    /// </summary>
    public string Permission { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 数据权限
    /// </summary>
    public DataScopeType DataScopeType { get; set; }
}
