using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Core.Message.Email;
using Bear.Core.Models.Base;
using Bear.Core.SqlSugar;

namespace Bear.Core.Models;

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

