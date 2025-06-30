using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Model;

namespace BearPlatform.ViewModel.Report.System;

/// <summary>
/// 字典导出模板
/// </summary>
public class DictExport : ExportBase
{
    /// <summary>
    /// 字典类型
    /// </summary>
    [Display(Name = "Dict.Type")]
    public DictType DictType { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [Display(Name = "Dict.Name")]
    public string Name { get; set; }

    /// <summary>
    /// 字典描述
    /// </summary>
    [Display(Name = "Sys.Description")]
    public string Description { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    [Display(Name = "Dict.Detail.Label")]
    public string Lable { get; set; }

    /// <summary>
    /// 字典值
    /// </summary>
    [Display(Name = "Dict.Detail.Value")]
    public string Value { get; set; }
}
