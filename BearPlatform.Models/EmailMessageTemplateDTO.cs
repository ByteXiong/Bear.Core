using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;

namespace BearPlatform.Models;

/// <summary>
/// 邮件消息模板
/// </summary>
#region 查询参数
public class EmailMessageTemplateParam : PageParam
{

}
#endregion

#region DTO
/// <summary>
///  分页
/// </summary>
[AutoMapping(typeof(EmailMessageTemplateDTO), typeof(EmailMessageTemplate))]
public class EmailMessageTemplateDTO : RootKeyDTO<long>
{


}

/// <summary>
/// 详情
/// </summary>
[AutoMapping(typeof(EmailMessageTemplateInfo), typeof(EmailMessageTemplate))]
public class EmailMessageTemplateInfo : EmailMessageTemplate
{

}
/// <summary>
/// 更新
/// </summary>
[AutoMapping(typeof(UpdateEmailMessageTemplateParam), typeof(EmailMessageTemplate))]
public class UpdateEmailMessageTemplateParam : EmailMessageTemplate
{
}
#endregion 

