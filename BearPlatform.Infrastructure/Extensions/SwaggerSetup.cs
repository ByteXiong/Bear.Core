using System.Runtime.InteropServices;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Helper.Serilog;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.Infrastructure.ActionFilter;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BearPlatform.Infrastructure.Extensions;

/// <summary>
/// swaggerSetup启动器
/// </summary>
public static class SwaggerSetup
{
    private static readonly ILogger Logger = SerilogManager.GetLogger(typeof(SwaggerSetup));

    public static void AddSwaggerSetup(this IServiceCollection services)
    {
        if (services.IsNull()) throw new ArgumentNullException(nameof(services));

        var basePath = AppContext.BaseDirectory;
        var swaggerOptions = App.GetOptions<Core.ConfigOptions.SwaggerOptions>();

        #region 配置版本管理
        services.AddApiVersioning(option =>
        {
            //版本号以什么形式，什么字段传递
            option.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("api-version"));

            // 在不提供版本号时，默认为1.0  如果不添加此配置，不提供版本号时会报错"message": "An API version is required, but was not specified."
            option.AssumeDefaultVersionWhenUnspecified = true;

            // 可选，为true时API返回支持的版本信息
            option.ReportApiVersions = true;
            // 请求中未指定版本时默认为1.0
            option.DefaultApiVersion = new ApiVersion(0, 0);

            //option.ErrorResponses
            //option.ErrorResponses = new MyErrorResponseProvider();
            //默认以当前最高版本进行访问
            //option.ApiVersionSelector = new CurrentImplementationApiVersionSelector(option);
        })
            .AddApiExplorer(opt =>
            {
                //以通知swagger替换控制器路由中的版本并配置api版本
                opt.SubstituteApiVersionInUrl = true;
                // 版本名的格式：v+版本号
                opt.GroupNameFormat = "'v'VVV";
                //是否提供API版本服务
                opt.AssumeDefaultVersionWhenUnspecified = true;

            });

        #endregion


        services.AddSwaggerGen();


        //解决上面报ASP0000警告的方案
        services.AddOptions<SwaggerGenOptions>()
                 .Configure<IApiVersionDescriptionProvider>((options, service) =>
                 {
                     options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
                     options.SchemaFilter<CustomSchemaFilter>();//swagger自定义
                                                                // 添加文档信息
                     foreach (var item in service.ApiVersionDescriptions)
                     {

                         options.SwaggerDoc(item.GroupName, CreateInfoForApiVersion(item));
                     }
                     OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
                     {
                         var info = new OpenApiInfo()
                         {
                             //标题
                             Title = $"{swaggerOptions.Title} {((VersionEnum)description.ApiVersion.MajorVersion).GetDisplayName()}",
                             //当前版本
                             Version = description.ApiVersion.ToString(),
                             //文档说明
                             Description = @"",

                             ////联系方式
                             //Contact = new OpenApiContact() { Name = "标题", Email = "", Url = null },
                             ////许可证
                             //License = new OpenApiLicense() { Name = "文档", Url = new Uri("") }
                         };
                         //当有弃用标记时的提示信息
                         if (description.IsDeprecated)
                         {
                             info.Description += " - 此版本已放弃兼容";
                         }
                         return info;
                     }
                     options.AddSecurityRequirement(new OpenApiSecurityRequirement
                         {

                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        }
                                    },
                                    new string[] { }
                                }
                         });


                     // 开启加权小锁
                     options.OperationFilter<AddResponseHeadersFilter>();
                     options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                     // 在header中添加token，传递到后台
                     options.OperationFilter<SecurityRequirementsOperationFilter>();

                     // JWT认证                                                 
                     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                     {
                         Scheme = "bearer",
                         BearerFormat = "JWT",
                         In = ParameterLocation.Header,
                         Description = "Authorization:Bearer {your JWT token}<br/><b>授权地址:/Test/Login</b>",
                         Name = "Authorization", //jwt默认的参数名称
                         Type = SecuritySchemeType.ApiKey


                     });



                     //给swagger添加过滤器
                     //options.OperationFilter<SwaggerParameterFilter>();
                     // 加载XML注释
                     // 为 Swagger JSON and UI设置xml文档注释路径
                     //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                     //var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                     var xmls = Directory.GetFiles(basePath, "*.xml");
                     Array.ForEach(xmls, aXml =>
                     {
                         options.IncludeXmlComments(aXml, true);
                     });
                     options.OrderActionsBy(o => o.RelativePath);
                 });

    }

    // public class ExcludeSchemaFilter : ISchemaFilter
    // {
    //     public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    //     {
    //         if (context.Type == typeof(System.Data.DbType))
    //         {
    //             // 将此类型标记为不应生成完整架构
    //             schema.Reference = null;
    //         }
    //     }
    // }
}
