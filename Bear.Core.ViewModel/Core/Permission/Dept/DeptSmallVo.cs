using Bear.Core.Common.Attributes;

namespace Bear.Core.ViewModel.Core.Permission.Dept;

/// <summary>
/// 部门Vo
/// </summary>
[AutoMapping(typeof(Entity.Core.Permission.Dept), typeof(DeptSmallVo))]
public class DeptSmallVo
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
