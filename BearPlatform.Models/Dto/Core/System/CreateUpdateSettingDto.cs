using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.System;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.System;

/// <summary>
/// 全局设置Dto
/// </summary>
[AutoMapping(typeof(Setting), typeof(param))]
public class param : BaseEntityDTO<long>
{
    /// <summary>
    /// 键
    /// </summary>
    [Display(Name = "Setting.Name")]
    [Required(ErrorMessage = "{0}required")]
    public string Name { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [Display(Name = "Setting.Value")]
    [Required(ErrorMessage = "{0}required")]
    public string Value { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Display(Name = "Sys.Description")]
    public string Description { get; set; }
}
