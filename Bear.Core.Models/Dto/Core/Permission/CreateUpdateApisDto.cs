using System.ComponentModel.DataAnnotations;
using Bear.Core.Common.Attributes;
using Bear.Core.Entity;
using Bear.Core.Models.Base;

namespace Bear.Core.Models.Dto.Core.Permission;

/// <summary>
/// 
/// </summary>
[AutoMapping(typeof(Apis), typeof(CreateUpdateApisDto))]
public class CreateUpdateApisDto : BaseEntityDTO<Guid>
{
    /// <summary>
    /// 组
    /// </summary>
    [Display(Name = "Api.Group")]
    [Required(ErrorMessage = "{0}required")]
    public string Group { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    [Display(Name = "Api.Url")]
    [Required(ErrorMessage = "{0}required")]
    public string Url { get; set; }


    /// <summary>
    /// 描述
    /// </summary>
    [Display(Name = "Sys.Description")]
    [Required(ErrorMessage = "{0}required")]
    public string Description { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    [Display(Name = "Api.Method")]
    [Required(ErrorMessage = "{0}required")]
    public string Method { get; set; }
}
