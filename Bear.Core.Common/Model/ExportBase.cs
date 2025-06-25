using System;
using System.ComponentModel.DataAnnotations;

namespace Bear.Core.Common.Model;

public class ExportBase
{
    /// <summary>
    /// 创建时间
    /// </summary>
    [Display(Name = "Sys.CreateTime")]
    public long CreateTime { get; set; }

    /// <summary>
    /// ID
    /// </summary>
    [Display(Name = "Sys.Id")]
    public long Id { get; set; }
}
