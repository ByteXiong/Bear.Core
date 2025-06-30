using System.ComponentModel.DataAnnotations;

namespace BearPlatform.Models.Dto.Core.Permission.User;

/// <summary>
/// 用户密码Dto
/// </summary>
public class UpdateUserPassDto
{
    /// <summary>
    /// 旧密码
    /// </summary>
    [Display(Name = "User.OldPassword")]
    [Required(ErrorMessage = "{0}required")]
    public string OldPassword { get; set; }

    /// <summary>
    /// 新密码
    /// </summary>
    [Display(Name = "User.NewPassword")]
    [Required(ErrorMessage = "{0}required")]
    public string NewPassword { get; set; }

    /// <summary>
    /// 确认新密码
    /// </summary>
    [Display(Name = "User.ConfirmPassword")]
    [Required(ErrorMessage = "{0}required")]
    //[Compare("NewPassword", ErrorMessage = "User.FailedVerificationTwice")]
    public string ConfirmPassword { get; set; }
}
