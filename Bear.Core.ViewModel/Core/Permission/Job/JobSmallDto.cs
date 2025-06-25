using Bear.Core.Common.Attributes;

namespace Bear.Core.ViewModel.Core.Permission.Job;

/// <summary>
/// 岗位Vo
/// </summary>
[AutoMapping(typeof(Entity.Core.Permission.Job), typeof(JobSmallDto))]
public class JobSmallDto
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
}
