using Bear.Core.Common.Attributes;
using Bear.Core.Entity.Core.System;
using Bear.Core.Models.Base;

namespace Bear.Core.ViewModel.Core.System;

/// <summary>
/// 全局设置Vo
/// </summary>
[AutoMapping(typeof(Setting), typeof(SettingVo))]
public class SettingVo : BaseEntityDTO<long>
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
