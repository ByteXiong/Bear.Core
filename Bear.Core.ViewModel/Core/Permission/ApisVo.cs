using Bear.Core.Common.Attributes;
using Bear.Core.Entity;
using Bear.Core.Entity.Core.Permission;
using Bear.Core.Models.Base;

namespace Bear.Core.ViewModel.Core.Permission;

/// <summary>
/// Api路由 Vo
/// </summary>
[AutoMapping(typeof(Apis), typeof(ApisVo))]
public class ApisVo : BaseEntityDTO<long>
{
    /// <summary>
    /// 组
    /// </summary>
    public string Group { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    public string Url { get; set; }


    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    public string Method { get; set; }
}
