using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.Models.Base;

namespace BearPlatform.ViewModel.Core.Message.Email;

/// <summary>
/// 邮箱模板Vo
/// </summary>
[AutoMapping(typeof(EmailMessageTemplate), typeof(EmailMessageTemplateVo))]
public class EmailMessageTemplateVo : BaseEntityDTO<long>
{
    /// <summary>
    /// 模板名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 抄送邮箱地址
    /// </summary>
    public string BccEmailAddresses { get; set; }

    /// <summary>
    /// 主题
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// 是否激活
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// 发送邮箱账户
    /// </summary>
    public long EmailAccountId { get; set; }
}
