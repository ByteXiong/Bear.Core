using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.Permission;

/// <summary>
/// 部门Dto
/// </summary>
[AutoMapping(typeof(Dept), typeof(CreateUpdateDeptDto))]
public class CreateUpdateDeptDto : BaseEntityDTO<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    [Display(Name = "Sys.Name")]
    [Required(ErrorMessage = "{0}required")]
    public string Name { get; set; }

    /// <summary>
    /// 父级ID
    /// </summary>
    public long ParentId { get; set; }

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
