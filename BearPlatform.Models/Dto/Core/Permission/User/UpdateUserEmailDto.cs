using System.ComponentModel.DataAnnotations;

namespace BearPlatform.Models.Dto.Core.Permission.User;

/// <summary>
/// 用户邮箱Dto
/// </summary>
public class UpdateUserEmailDto
{
    /// <summary>
    /// 密码
    /// </summary>
    [Display(Name = "User.Password")]
    [Required(ErrorMessage = "{0}required")]
    public string Password { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [Display(Name = "Sys.Email")]
    [Required(ErrorMessage = "{0}required")]
    [EmailAddress(ErrorMessage = "{0}Error.Format")]
    public string Email { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    [Display(Name = "Sys.Captcha")]
    [Required(ErrorMessage = "{0}required")]
    public string Code { get; set; }
}
