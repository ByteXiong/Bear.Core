using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Queued;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;

namespace BearPlatform.Models;


/// <summary>
/// 邮件队列
/// </summary>
#region 查询参数
public class QueuedEmailParam : PageParam
{

}
#endregion

#region DTO
/// <summary>
///  分页
/// </summary>
[AutoMapping(typeof(QueuedEmailDTO), typeof(QueuedEmail))]
public class QueuedEmailDTO : RootKeyDTO<long>
{


}

/// <summary>
/// 详情
/// </summary>
[AutoMapping(typeof(QueuedEmailInfo), typeof(QueuedEmail))]
public class QueuedEmailInfo : QueuedEmail
{

}
/// <summary>
/// 更新
/// </summary>
[AutoMapping(typeof(UpdateQueuedEmailParam), typeof(QueuedEmail))]
public class UpdateQueuedEmailParam : QueuedEmail
{
}
#endregion 

