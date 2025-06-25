using System.IdentityModel.Tokens.Jwt;
using Bear.Core.Common.WebApp;
using Bear.Core.ViewModel.Jwt;

namespace Bear.Core.Infrastructure.Authentication;

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
