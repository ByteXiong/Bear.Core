using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Model;

namespace BearPlatform.ViewModel.Report.System;

/// <summary>
/// 设置导出模板
/// </summary>
public class SettingExport : ExportBase
{
    /// <summary>
    /// 键
    /// </summary>
    [Display(Name = "Setting.Name")]
    public string Name { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [Display(Name = "Setting.Value")]
    public string Value { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [Display(Name = "Setting.Enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Display(Name = "Sys.Description")]
    public string Description { get; set; }
}
