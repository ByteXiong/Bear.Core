using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;

namespace BearPlatform.Models.Queries.Common;

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
