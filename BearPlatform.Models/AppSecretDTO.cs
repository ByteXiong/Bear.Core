using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.System;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;

namespace BearPlatform.Models;


/// <summary>
/// App应用秘钥
/// </summary>
#region 查询参数
public class AppSecretParam : PageParam
{

}
#endregion

#region DTO
/// <summary>
///  分页
/// </summary>
[AutoMapping(typeof(AppSecretDTO), typeof(AppSecret))]
public class AppSecretDTO : RootKeyDTO<long>
{


}

/// <summary>
/// 详情
/// </summary>
[AutoMapping(typeof(AppSecretInfo), typeof(AppSecret))]
public class AppSecretInfo : AppSecret
{

}
/// <summary>
/// 更新
/// </summary>
[AutoMapping(typeof(UpdateAppSecretParam), typeof(AppSecret))]
public class UpdateAppSecretParam : AppSecret
{
}
#endregion 

