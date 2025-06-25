using System.ComponentModel.DataAnnotations;
using Bear.Core.Common.Attributes;
using Bear.Core.Entity;
using Bear.Core.Entity.Core.Permission;

namespace Bear.Core.Models.Dto.Core.Permission.Role;

/// <summary>
/// 角色菜单Dto
/// </summary>
[AutoMapping(typeof(Apis), typeof(RoleApisDto))]
public class RoleApisDto
{
    /// <summary>
    /// ID
    /// </summary>
    [RegularExpression(@"^\+?[1-9]\d*$")]
    public Guid Id { get; set; }
}
