using Bear.Core.Models;
using Bear.Core.ViewModel.Core.Permission.User;

namespace Bear.Core.ViewModel.Jwt;

/// <summary>
/// JWT令牌用户
/// </summary>
public class JwtUserInfo
{
    /// <summary>
    /// 用户
    /// </summary>
    public UserInfo User { get; set; }

    /// <summary>
    /// 角色权限
    /// </summary>
    public List<string> Roles { get; set; }

    /// <summary>
    /// 数据权限
    /// </summary>
    public List<string> DataScopes { get; set; }
}
