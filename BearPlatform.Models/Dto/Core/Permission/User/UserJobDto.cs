using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Permission;

namespace BearPlatform.Models.Dto.Core.Permission.User;

/// <summary>
/// 用户岗位Dto
/// </summary>
[AutoMapping(typeof(Job), typeof(UserJobDto))]
public class UserJobDto
{
    /// <summary>
    /// ID
    /// </summary>
    [Display(Name = "Sys.Id")]
    [RegularExpression(@"^\+?[1-9]\d*$", ErrorMessage = "{0}Error.Format")]
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Display(Name = "Sys.Name")]
    [Required(ErrorMessage = "{0}required")]
    public string Name { get; set; }
}
