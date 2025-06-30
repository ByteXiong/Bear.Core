using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;

namespace BearPlatform.Models.Dto.Core.Permission.Role;

/// <summary>
/// 更新角色Api
/// </summary>
public class UpdateRoleApiDto
{
    /// <summary>
    /// 角色ID
    /// </summary>
    [Display(Name = "Sys.ID")]
    [RegularExpression(@"^\+?[1-9]\d*$")]
    public long Id { get; set; }

    /// <summary>
    /// 角色菜单
    /// </summary>
    [Display(Name = "Sys.Api")]
    [Required(ErrorMessage = "{0}required")]
    [AtLeastOneItem]
    public List<RoleApisDto> Apis { get; set; }
}
