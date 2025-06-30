using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.Message.Email;

/// <summary>
/// 邮箱模板Dto
/// </summary>
[AutoMapping(typeof(EmailMessageTemplate), typeof(CreateUpdateEmailMessageTemplateDto))]
public class CreateUpdateEmailMessageTemplateDto : BaseEntityDTO<long>
{
    /// <summary>
    /// 模板名称
    /// </summary>
    [Display(Name = "EmailTemplate.Name")]
    [Required(ErrorMessage = "{0}required")]
    public string Name { get; set; }

    /// <summary>
    /// 抄送邮箱地址
    /// </summary>
    public string BccEmailAddresses { get; set; }

    /// <summary>
    /// 主题
    /// </summary>
    [Display(Name = "EmailTemplate.Subject")]
    [Required(ErrorMessage = "{0}required")]
    public string Subject { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [Display(Name = "EmailTemplate.Body")]
    [Required(ErrorMessage = "{0}required")]
    public string Body { get; set; }

    /// <summary>
    /// 是否激活
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 邮箱账户标识符
    /// </summary>
    public long EmailAccountId { get; set; }
}
