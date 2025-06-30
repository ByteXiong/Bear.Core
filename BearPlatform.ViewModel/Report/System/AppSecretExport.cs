using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Model;

namespace BearPlatform.ViewModel.Report.System;

/// <summary>
/// 密钥导出模板
/// </summary>
public class AppSecretExport : ExportBase
{
    /// <summary>
    /// 应用ID
    /// </summary>
    [Display(Name = "App.AppId")]
    public string AppId { get; set; }

    /// <summary>
    /// 应用密钥
    /// </summary>
    [Display(Name = "App.AppSecretKey")]
    public string AppSecretKey { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [Display(Name = "App.AppName")]
    public string AppName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Display(Name = "App.Remark")]
    public string Remark { get; set; }
}
