using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;

namespace BearPlatform.Models.Queries.Common;

/// <summary>
/// id模型(log)
/// </summary>
public class IdCollection
{
    /// <summary>
    /// ids
    /// </summary>
    [Display(Name = "Sys.Id")]
    [Required(ErrorMessage = "{0}required")]
    [AtLeastOneItem]
    public HashSet<long> IdArray { get; set; }
}
