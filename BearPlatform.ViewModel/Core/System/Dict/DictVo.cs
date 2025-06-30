using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Models.Base;

namespace BearPlatform.ViewModel.Core.System.Dict;

/// <summary>
/// 字典Vo
/// </summary>
[AutoMapping(typeof(Entity.Core.System.Dict.Dict), typeof(DictVo))]
public class DictVo : BaseEntityDTO<long>
{
    /// <summary>
    /// 字典类型
    /// </summary>
    /// <returns></returns>
    public DictType DictType { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 字典详情
    /// </summary>
    public List<DictDetailVo> DictDetails { get; set; }
}
