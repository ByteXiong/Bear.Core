using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity;
using BearPlatform.Entity.Core.Permission;

namespace BearPlatform.Models.Dto.Core.Permission.Role;

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
