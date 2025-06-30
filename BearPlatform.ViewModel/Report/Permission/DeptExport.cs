using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Model;

namespace BearPlatform.ViewModel.Report.Permission;

/// <summary>
/// 部门导出模板
/// </summary>
public class DeptExport : ExportBase
{
    /// <summary>
    /// 部门名称
    /// </summary>
    [Display(Name = "Dept.Name")]
    public string Name { get; set; }

    /// <summary>
    /// 部门父ID
    /// </summary>
    [Display(Name = "Dept.PId")]
    public long ParentId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Display(Name = "Sys.Sort")]
    public int Sort { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    [Display(Name = "Dept.Enabled")]
    public bool Enabled { get; set; }

    /// <summary>
    /// 子部门个数
    /// </summary>
    [Display(Name = "Dept.SubCount")]
    public int SubCount { get; set; }
}
