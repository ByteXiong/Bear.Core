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
/// 全局配置
/// </summary>
#region 查询参数
public class SettingParam : PageParam
{

}
#endregion

#region DTO
/// <summary>
///  分页
/// </summary>
[AutoMapping(typeof(SettingDTO), typeof(Setting))]
public class SettingDTO : RootKeyDTO<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

}

/// <summary>
/// 详情
/// </summary>
[AutoMapping(typeof(SettingInfo), typeof(Setting))]
public class SettingInfo : Setting
{

}
/// <summary>
/// 更新
/// </summary>
[AutoMapping(typeof(UpdateSettingParam), typeof(Setting))]
public class UpdateSettingParam : Setting
{
}
#endregion 

