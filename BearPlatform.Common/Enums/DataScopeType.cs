using System.ComponentModel.DataAnnotations;

namespace BearPlatform.Common.Enums;

public enum DataScopeType
{
    /// <summary>
    /// 无
    /// </summary>
    [Display(Name = "None")]
    None = 0,

    /// <summary>
    /// 全部
    /// </summary>
    [Display(Name = "Enum.DataScope.All")]
    All = 1,

    /// <summary>
    /// 本人
    /// </summary>
    [Display(Name = "Enum.DataScope.MySelf")]
    MySelf = 2,

    /// <summary>
    /// 本部门
    /// </summary>
    [Display(Name = "Enum.DataScope.MyDept")]
    MyDept = 3,

    /// <summary>
    /// 本部门及以下
    /// </summary>
    [Display(Name = "Enum.DataScope.MyDeptAndBelow")]
    MyDeptAndBelow = 4,

    /// <summary>
    /// 自定义
    /// </summary>
    [Display(Name = "Enum.DataScope.Customize")]
    Customize = 5
}
