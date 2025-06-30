using BearPlatform.Common.Attributes;
using BearPlatform.Models.Base;

namespace BearPlatform.ViewModel.Core.Permission.Job;

/// <summary>
/// 岗位Vo
/// </summary>
[AutoMapping(typeof(Entity.Core.Permission.Job), typeof(JobVo))]
public class JobVo : BaseEntityDTO<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }
}
