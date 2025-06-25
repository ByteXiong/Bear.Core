using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Core.Queued;
using Bear.Core.Models.Base;
using Bear.Core.SqlSugar;

namespace Bear.Core.Models;


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

