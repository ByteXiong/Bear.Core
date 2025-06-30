using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Permission;

namespace BearPlatform.Models.Dto.Core.Permission.User;

/// <summary>
/// 用户部门Dto
/// </summary>
[AutoMapping(typeof(Dept), typeof(UserDeptDto))]
public class UserDeptDto
{
    /// <summary>
    /// ID
    /// </summary>
    [RegularExpression(@"^\+?[1-9]\d*$")]
    public long Id { get; set; }
}
