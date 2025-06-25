using System.ComponentModel.DataAnnotations;

namespace Bear.Core.Common.Enums;

public enum DictType
{
    /// <summary>
    /// 系统类
    /// </summary>
    [Display(Name = "Enum.Dict.System")]
    System = 1,

    /// <summary>
    /// 业务类
    /// </summary>
    [Display(Name = "Enum.Dict.Business")]
    Business = 2
}
