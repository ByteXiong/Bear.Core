using Newtonsoft.Json;

namespace BearPlatform.ViewModel.Jwt;

/// <summary>
/// 
/// </summary>
public class LoginToken
{
    /// <summary>
    /// 授权token
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// 过期时间
    /// </summary>
    public int Expires { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    public string TokenType { get; set; }

    /// <summary>
    /// 刷新token
    /// </summary>
    public string RefreshToken { get; set; }

    /// <summary>
    /// 允许token时间内
    /// </summary>
    public int RefreshTokenExpires { get; set; }
}
