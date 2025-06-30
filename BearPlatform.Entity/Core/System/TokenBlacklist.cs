using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity.Core.System
{
    /// <summary>
    /// Token黑名单
    /// </summary>
    [SugarTable("sys_token_blacklist")]
    public class TokenBlacklist : BaseEntity<long>
    {
        /// <summary>
        /// 令牌 登录token的MD5值
        /// </summary>
        public string AccessToken { get; set; }
    }
}
