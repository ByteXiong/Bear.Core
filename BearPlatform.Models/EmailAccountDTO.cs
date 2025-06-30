using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;

namespace BearPlatform.Models;


/// <summary>
/// 邮箱账户管理
/// </summary>
#region 查询参数
public class EmailAccountParam : PageParam
{

}
#endregion

#region DTO
/// <summary>
///  分页
/// </summary>
[AutoMapping(typeof(EmailAccountDTO), typeof(EmailAccount))]
public class EmailAccountDTO : RootKeyDTO<long>
{


}

/// <summary>
/// 详情
/// </summary>
[AutoMapping(typeof(EmailAccountInfo), typeof(EmailAccount))]
public class EmailAccountInfo : EmailAccount
{

}
/// <summary>
/// 更新
/// </summary>
[AutoMapping(typeof(UpdateEmailAccountParam), typeof(EmailAccount))]
public class UpdateEmailAccountParam : EmailAccount
{
}
#endregion 

