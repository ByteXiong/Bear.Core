using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Entity.Permission;

namespace BearPlatform.Models.Dto.Core.Permission.Role;

/// <summary>
/// 角色菜单Dto
/// </summary>
[AutoMapping(typeof(Menu), typeof(RoleMenuDto))]
public class RoleMenuDto
{
    /// <summary>
    /// ID
    /// </summary>
    [Display(Name = "Sys.Id")]
    [RegularExpression(@"^\+?[1-9]\d*$")]
    public long Id { get; set; }
}
