using System.IdentityModel.Tokens.Jwt;
using BearPlatform.Common.WebApp;
using BearPlatform.ViewModel.Jwt;

namespace BearPlatform.Infrastructure.Authentication;

public interface ITokenService
{
    /// <summary>
    /// Issue token
    /// </summary>
    /// <param name="loginUserInfo"></param>
    /// <param name="refresh"></param>
    /// <returns></returns>
    Task<LoginToken> IssueTokenAsync(LoginUserInfo loginUserInfo, bool refresh = false);

    Task<JwtSecurityToken> ReadJwtToken(string token);
}
