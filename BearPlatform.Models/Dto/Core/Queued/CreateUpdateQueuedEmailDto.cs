using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Queued;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.Queued;

/// <summary>
/// 邮件队列Dto
/// </summary>
[AutoMapping(typeof(QueuedEmail), typeof(CreateUpdateQueuedEmailDto))]
public class CreateUpdateQueuedEmailDto : BaseEntityDTO<long>
{
    /// <summary>
    /// 发件邮箱
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// 发件人名称
    /// </summary>
    public string FromName { get; set; }

    /// <summary>
    /// 收件邮箱
    /// </summary>
    [Display(Name = "QueuedEmail.To")]
    [Required(ErrorMessage = "{0}required")]
    public string To { get; set; }

    /// <summary>
    /// 收件人名称
    /// </summary>
    public string ToName { get; set; }

    /// <summary>
    /// 回复邮箱
    /// </summary>
    public string ReplyTo { get; set; }

    /// <summary>
    /// 回复人名称
    /// </summary>
    public string ReplyToName { get; set; }

    /// <summary>
    /// 优先级
    /// </summary>
    [Display(Name = "QueuedEmail.Priority")]
    [Range(0, 1, ErrorMessage = "{0}range{1}{2}")]
    public int Priority { get; set; }

    /// <summary>
    /// 抄送
    /// </summary>
    public string Cc { get; set; }

    /// <summary>
    /// 密件抄送
    /// </summary>
    public string Bcc { get; set; }

    /// <summary>
    /// 标题
    /// </summary>
    [Display(Name = "QueuedEmail.Subject")]
    [Required(ErrorMessage = "{0}required")]
    public string Subject { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [Display(Name = "QueuedEmail.Body")]
    [Required(ErrorMessage = "{0}required")]
    public string Body { get; set; }

    /// <summary>
    /// 发送上限次数
    /// </summary>
    public int SentTries { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SendTime { get; set; }

    /// <summary>
    /// 发件邮箱ID
    /// </summary>
    [Display(Name = "QueuedEmail.EmailAccountId")]
    public long EmailAccountId { get; set; }
}
