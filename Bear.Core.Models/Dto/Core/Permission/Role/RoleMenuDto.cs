using System.ComponentModel.DataAnnotations;
using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Core.Permission;
using Bear.Core.Entity.Permission;

namespace Bear.Core.Models.Dto.Core.Permission.Role;

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
