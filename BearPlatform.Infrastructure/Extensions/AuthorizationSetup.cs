using System.Text;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.WebApp;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BearPlatform.Infrastructure.Extensions;

/// <summary>
/// 授权启动器
/// </summary>
public static class AuthorizationSetup
{
    public static void AddAuthorizationSetup(this IServiceCollection services)
    {
        if (services.IsNull()) throw new ArgumentNullException(nameof(services));


        services.AddScoped<IHttpUser, HttpUser>();
        services.AddScoped<ITokenService, TokenService>();

        var jwtAuthOptions = App.GetOptions<JwtAuthOptions>();

        var permissionRequirement = new PermissionRequirement();
        // 自定义策略授权
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthConstants.AuthPolicyName,
                policy => policy.Requirements.Add(permissionRequirement));
        });

        // 开启Bearer认证
        services.AddAuthentication(o =>
            {
                o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = nameof(ApiResponseHandler);
                o.DefaultForbidScheme = nameof(ApiResponseHandler);
            })
            // 添加JwtBearer服务
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // NameClaimType = AuthConstants.JwtClaimTypes.Name,
                    // RoleClaimType = AuthConstants.JwtClaimTypes.Role,

                    ValidateIssuer = true,
                    ValidIssuer = jwtAuthOptions.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtAuthOptions.Audience,

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtAuthOptions.SecurityKey)),
                    LifetimeValidator = (DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
                        TokenValidationParameters validationParameters) =>
                    {
                        if (expires == null)
                        {
                            return true;
                        }

                        return expires.Value > DateTime.UtcNow;
                    },
                    ValidateLifetime = true
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        // 如果过期，把过期信息添加到头部
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Append("Token-Expired", "true");
                        }

                        return Task.CompletedTask;
                    }
                };
            })
            .AddScheme<AuthenticationSchemeOptions, ApiResponseHandler>(nameof(ApiResponseHandler), _ => { });


        services.AddScoped<IAuthorizationHandler, PermissionHandler>();
        services.AddSingleton(permissionRequirement);
    }
}
