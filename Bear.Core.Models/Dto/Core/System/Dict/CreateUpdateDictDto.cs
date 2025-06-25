using System.ComponentModel.DataAnnotations;
using Bear.Core.Common.Attributes;
using Bear.Core.Common.Enums;
using Bear.Core.Models.Base;

namespace Bear.Core.Models.Dto.Core.System.Dict;

/// <summary>
/// 字典Dto
/// </summary>
[AutoMapping(typeof(Entity.Core.System.Dict.Dict), typeof(CreateUpdateDictDto))]
public class CreateUpdateDictDto : BaseEntityDTO<long>
{
    /// <summary>
    /// 字典类型
    /// </summary>
    [Display(Name = "Dict.Type")]
    [Range(1, 2, ErrorMessage = "{0}range{1}{2}")]
    public DictType DictType { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Display(Name = "Dict.Name")]
    [Required(ErrorMessage = "{0}required")]
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [Display(Name = "Dict.Description")]
    [Required(ErrorMessage = "{0}required")]
    public string Description { get; set; }
}
