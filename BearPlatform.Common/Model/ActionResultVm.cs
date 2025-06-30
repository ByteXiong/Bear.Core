using Microsoft.AspNetCore.Http;

namespace BearPlatform.Common.Model;

/// <summary>
/// 请求响应结果
/// </summary>
public class ActionResultVm
{
    /// <summary>
    /// 状态码
    /// </summary>
    public int Status { get; set; } = StatusCodes.Status200OK;

    /// <summary>
    /// 错误集合
    /// </summary>
    public ActionError ActionError { get; set; }

    /// <summary>
    /// 默认显示的错误消息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 时间戳(毫秒)
    /// </summary>
    public string Timestamp { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    public string Path { get; set; }
}
