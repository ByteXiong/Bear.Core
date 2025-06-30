using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.System.Dict;
using BearPlatform.Models.Base;

namespace BearPlatform.ViewModel.Core.System.Dict;

/// <summary>
/// 字典详情Vo
/// </summary>
[AutoMapping(typeof(DictDetail), typeof(DictDetailVo))]
public class DictDetailVo : BaseEntityDTO<long>
{
    /// <summary>
    /// 字典ID
    /// </summary>
    //[JsonIgnore]
    //[JsonProperty]
    public long DictId { get; set; }

    /// <summary>
    /// 标签
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int DictSort { get; set; }
}
