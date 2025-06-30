using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Permission;

namespace BearPlatform.Models.Dto.Core.Permission.Role;

/// <summary>
/// 角色部门Dto
/// </summary>
[AutoMapping(typeof(Dept), typeof(RoleDeptDto))]
public class RoleDeptDto
{
    /// <summary>
    /// ID
    /// </summary>
    [RegularExpression(@"^\+?[1-9]\d*$")]
    public long Id { get; set; }
}
