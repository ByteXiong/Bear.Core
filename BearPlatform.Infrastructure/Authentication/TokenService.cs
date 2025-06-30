using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.WebApp;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.ViewModel.Jwt;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace BearPlatform.Infrastructure.Authentication;

public class TokenService : ITokenService
{
    private readonly ILogger<TokenService> _logger;


    public TokenService(ILogger<TokenService> logger)
    {
        _logger = logger;
    }

    public async Task<LoginToken> IssueTokenAsync(LoginUserInfo loginUserInfo, bool refresh = false)
    {
        if (loginUserInfo == null)
            throw new ArgumentNullException(nameof(loginUserInfo));

        var jwtAuthOptions = App.GetOptions<JwtAuthOptions>();
        var signinCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthOptions.SecurityKey)),
                SecurityAlgorithms.HmacSha256);
        var nowTime = DateTime.Now;
        var cls = new List<Claim>
        {
            new(AuthConstants.JwtClaimTypes.Jti, loginUserInfo.UserId.ToString()),
            new(AuthConstants.JwtClaimTypes.Name, loginUserInfo.Account),
            new(AuthConstants.JwtClaimTypes.TenantId, loginUserInfo.TenantId.ToString()),
            new(AuthConstants.JwtClaimTypes.DeptId, loginUserInfo.DeptId.ToString()),
            new(AuthConstants.JwtClaimTypes.Iat, nowTime.ToUnixTimeStampSecond().ToString()),
            new(AuthConstants.JwtClaimTypes.Ip, loginUserInfo.Ip)
        };
        var identity = new ClaimsIdentity(AuthConstants.JwtTokenType);
        identity.AddClaims(cls);


        var tokeOptions = new JwtSecurityToken(
            issuer: jwtAuthOptions.Issuer,
            audience: jwtAuthOptions.Audience,
            claims: cls,
            notBefore: nowTime,
            expires: nowTime.AddHours(jwtAuthOptions.Expires),
            signingCredentials: signinCredentials
        );


        var token = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
        if (refresh)
        {
            return await Task.FromResult(new LoginToken
            {
                Expires = jwtAuthOptions.Expires * 3600,
                TokenType = AuthConstants.JwtTokenType,
                RefreshToken = token,
            });
        }

        return await Task.FromResult(new LoginToken
        {
            AccessToken = token,
            Expires = jwtAuthOptions.Expires * 3600,
            TokenType = AuthConstants.JwtTokenType,
            RefreshToken = "",
            RefreshTokenExpires = jwtAuthOptions.RefreshTokenExpires * 3600
        });
    }


    public async Task<JwtSecurityToken> ReadJwtToken(string token)
    {
        try
        {
            token = token.Replace(AuthConstants.JwtTokenType, "").Trim();
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            if (jwtSecurityTokenHandler.CanReadToken(token))
            {
                var jwtAuthOptions = App.GetOptions<JwtAuthOptions>();
                var signinCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthOptions.SecurityKey)),
                        SecurityAlgorithms.HmacSha256);
                JwtSecurityToken jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(token);
                var rawSignature = JwtTokenUtilities.CreateEncodedSignature(
                    jwtSecurityToken.RawHeader + "." + jwtSecurityToken.RawPayload,
                    signinCredentials);
                if (jwtSecurityToken.RawSignature == rawSignature)
                {
                    return await Task.FromResult(jwtSecurityToken);
                }
            }
        }
        catch (Exception e)
        {
            _logger.LogError("Error reading JWT token: {Message}", e.Message);
        }

        return null;
    }
}
