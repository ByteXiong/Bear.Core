using System.ComponentModel.DataAnnotations;
using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Core.Permission;

namespace Bear.Core.Models.Dto.Core.Permission.User;

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
