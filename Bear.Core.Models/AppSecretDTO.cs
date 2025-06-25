using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Core.System;
using Bear.Core.Models.Base;
using Bear.Core.SqlSugar;

namespace Bear.Core.Models;


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

