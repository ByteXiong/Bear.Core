using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Entity.Permission;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.Permission;

/// <summary>
/// 菜单Dto
/// </summary>
[AutoMapping(typeof(Menu), typeof(CreateUpdateMenuDto))]
public class CreateUpdateMenuDto : BaseEntityDTO<long>
{
    /// <summary>
    /// 标题
    /// </summary>
    [Display(Name = "Menu.Title")]
    [Required(ErrorMessage = "{0}required")]
    public string Title { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    [Display(Name = "Menu.Permission")]
    public string Permission { get; set; }

    /// <summary>
    /// 是否IFrame
    /// </summary>
    public bool IFrame { get; set; }

    /// <summary>
    /// 组件
    /// </summary>
    public string Component { get; set; }

    /// <summary>
    /// 组件名称
    /// </summary>
    [Display(Name = "Menu.ComponentName")]
    public string ComponentName { get; set; }

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
    /// Icon图标
    /// </summary>
    public string Icon { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [Display(Name = "Menu.Type")]
    [Range(1, 3, ErrorMessage = "{0}range{1}{2}")]
    public MenuType Type { get; set; }

    /// <summary>
    /// 缓存
    /// </summary>
    public bool Cache { get; set; }

    /// <summary>
    /// 隐藏
    /// </summary>
    public bool Hidden { get; set; }

    /// <summary>
    /// 子菜单个数
    /// </summary>
    public int SubCount { get; set; }
}
