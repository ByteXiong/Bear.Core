using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;

namespace BearPlatform.ViewModel.Core.Permission.User;

/// <summary>
/// 用户部门Vo
/// </summary>
[AutoMapping(typeof(Entity.Core.Permission.Dept), typeof(UserDeptVo))]
public class UserDeptVo
{
    /// <summary>
    /// ID
    /// </summary>
    [RegularExpression(@"^\+?[1-9]\d*$")]
    public long Id { get; set; }
}
