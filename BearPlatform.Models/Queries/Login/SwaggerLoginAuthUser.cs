using System.ComponentModel.DataAnnotations;

namespace BearPlatform.Models.Queries.Login;

/// <summary>
/// Swagger登录用户
/// </summary>
public class SwaggerLoginAuthUser
{
    /// <summary>
    /// 用户名
    /// </summary>
    [Display(Name = "User.UserName")]
    [Required(ErrorMessage = "{0}required")]
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Display(Name = "User.Password")]
    [Required(ErrorMessage = "{0}required")]
    public string Password { get; set; }
}
