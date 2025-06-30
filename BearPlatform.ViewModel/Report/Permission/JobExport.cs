using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Model;

namespace BearPlatform.ViewModel.Report.Permission;

/// <summary>
/// 岗位导出模板
/// </summary>
public class JobExport : ExportBase
{
    /// <summary>
    /// 岗位名称
    /// </summary>
    [Display(Name = "Job.Name")]
    public string Name { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Display(Name = "Sys.Sort")]
    public int Sort { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [Display(Name = "Job.Enabled")]
    public bool Enabled { get; set; }
}
