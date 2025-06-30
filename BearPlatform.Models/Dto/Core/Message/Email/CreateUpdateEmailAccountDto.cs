using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.Message.Email;

/// <summary>
/// 邮箱账户Dto
/// </summary>
[AutoMapping(typeof(EmailAccount), typeof(CreateUpdateEmailAccountDto))]
public class CreateUpdateEmailAccountDto : BaseEntityDTO<long>
{
    /// <summary>
    ///电子邮件地址
    /// </summary>
    [Display(Name = "Sys.Email")]
    [Required(ErrorMessage = "{0}required")]
    [EmailAddress(ErrorMessage = "{0}Error.Format")]
    public string Email { get; set; }

    /// <summary>
    /// 电子邮件显示名称
    /// </summary>
    [Display(Name = "EmailAccount.DisplayName")]
    [Required(ErrorMessage = "{0}required")]
    public string DisplayName { get; set; }

    /// <summary>
    /// 电子邮件主机
    /// </summary>
    [Display(Name = "EmailAccount.Host")]
    [Required(ErrorMessage = "{0}required")]
    public string Host { get; set; }

    /// <summary>
    /// 电子邮件端口
    /// </summary>
    [Display(Name = "EmailAccount.Port")]
    [Range(1, 65535, ErrorMessage = "{0}range{1}{2}")]
    public int Port { get; set; }

    /// <summary>
    /// 电子邮件用户名
    /// </summary>
    [Display(Name = "EmailAccount.UserName")]
    [Required(ErrorMessage = "{0}required")]
    public string UserName { get; set; }

    /// <summary>
    /// 电子邮件密码
    /// </summary>
    [Display(Name = "EmailAccount.Password")]
    [Required(ErrorMessage = "{0}required")]
    public string Password { get; set; }

    /// <summary>
    /// 是否SSL
    /// </summary>
    public bool EnableSsl { get; set; } = false;

    /// <summary>
    /// 是否与请求一起发送应用程序的默认系统凭据
    /// </summary>
    public bool UseDefaultCredentials { get; set; } = false;
}
