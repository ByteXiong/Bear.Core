using System.ComponentModel.DataAnnotations;

namespace BearPlatform.Models.Queries.Login;

/// <summary>
/// 登录用户
/// </summary>
public class LoginParam
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Required(ErrorMessage = "{0}required")]
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "{0}required")]
    public string Password { get; set; }

    /// <summary>
    /// 验证码
    /// </summary>
    public string Captcha { get; set; }

    /// <summary>
    /// 验证码ID
    /// </summary>
    public string CaptchaId { get; set; }
}
