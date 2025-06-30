using System.Collections.Generic;

namespace BearPlatform.Common.Model;

/// <summary>
/// 请求响应结果 泛型
/// </summary>
/// <typeparam name="T"></typeparam>
public class ActionResultVm<T> //: ActionResultVM
{
    /// <summary>
    /// 返回数据
    /// </summary>
    public List<T> Content { get; set; }

    /// <summary>
    /// 总条数
    /// </summary>
    public int TotalElements { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }
}
