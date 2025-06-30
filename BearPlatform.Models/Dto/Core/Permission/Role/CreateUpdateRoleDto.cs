using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.Permission.Role;

/// <summary>
/// 角色Dto
/// </summary>
[AutoMapping(typeof(Entity.Core.Permission.Role.Role), typeof(CreateUpdateRoleDto))]
public class CreateUpdateRoleDto : BaseEntityDTO<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    [Display(Name = "Sys.Name")]
    [Required(ErrorMessage = "{0}required")]
    public string Name { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    [Display(Name = "Role.Level")]
    [Range(1, 99, ErrorMessage = "{0}range{1}{2}")]
    public int Level { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Display(Name = "Sys.Description")]
    [Required(ErrorMessage = "{0}required")]
    public string Description { get; set; }

    /// <summary>
    /// 数据权限
    /// </summary>
    [Display(Name = "Role.DataScopeType")]
    [Range(0, 5, ErrorMessage = "{0}range{1}{2}")]
    public DataScopeType DataScopeType { get; set; }

    /// <summary>
    /// 标识
    /// </summary>
    [Display(Name = "Role.Permission")]
    [Required(ErrorMessage = "{0}required")]
    public string Permission { get; set; }

    /// <summary>
    /// 角色部门
    /// </summary>
    public List<RoleDeptDto> Depts { get; set; }
}
