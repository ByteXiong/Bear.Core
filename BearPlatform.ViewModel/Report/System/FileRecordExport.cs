using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Model;

namespace BearPlatform.ViewModel.Report.System;

/// <summary>
/// 文件记录导出模板
/// </summary>
public class FileRecordExport : ExportBase
{
    /// <summary>
    /// 文件描述
    /// </summary>
    [Display(Name = "Sys.Description")]
    public string Description { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    [Display(Name = "File.ContentType")]
    public string ContentType { get; set; }

    /// <summary>
    /// 文件类型名称
    /// </summary>
    [Display(Name = "File.ContentTypeName")]
    public string ContentTypeName { get; set; }

    /// <summary>
    /// 文件类型名称(EN)
    /// </summary>
    [Display(Name = "File.ContentTypeNameEn")]
    public string ContentTypeNameEn { get; set; }

    /// <summary>
    /// 源名称
    /// </summary>
    [Display(Name = "File.OriginalName")]
    public string OriginalName { get; set; }

    /// <summary>
    /// 新名称
    /// </summary>
    [Display(Name = "File.NewName")]
    public string NewName { get; set; }

    /// <summary>
    /// 存储路径
    /// </summary>
    [Display(Name = "File.FilePath")]
    public string FilePath { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    [Display(Name = "File.Size")]
    public string Size { get; set; }
}
