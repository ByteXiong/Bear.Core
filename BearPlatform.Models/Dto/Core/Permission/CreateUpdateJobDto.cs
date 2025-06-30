using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.Permission;

/// <summary>
/// 岗位Dto
/// </summary>
[AutoMapping(typeof(Job), typeof(param))]
public class param : BaseEntityDTO<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    [Display(Name = "Job.Name")]
    [Required(ErrorMessage = "{0}required")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 排序
    /// </summary>
    [Display(Name = "Sys.Sort")]
    [Range(1, 999, ErrorMessage = "{0}range{1}{2}")]
    public int Sort { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}
