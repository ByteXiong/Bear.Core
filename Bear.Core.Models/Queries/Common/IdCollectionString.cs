using System.ComponentModel.DataAnnotations;
using Bear.Core.Common.Attributes;

namespace Bear.Core.Models.Queries.Common;

/// <summary>
/// id模型(string)
/// </summary>
public class IdCollectionString
{
    /// <summary>
    /// 
    /// </summary>
    [Display(Name = "Sys.Id")]
    [Required(ErrorMessage = "{0}required")]
    [AtLeastOneItem]
    public HashSet<string> IdArray { get; set; }
}
