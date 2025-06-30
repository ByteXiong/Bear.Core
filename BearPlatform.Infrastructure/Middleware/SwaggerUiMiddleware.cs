using Asp.Versioning.ApiExplorer;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper.Serilog;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using Serilog;

namespace BearPlatform.Infrastructure.Middleware;


/// <summary>
/// Swagger UI 中间件
/// </summary>
public static class SwaggerUiMiddleware
{
    private static readonly ILogger Logger = SerilogManager.GetLogger(typeof(SwaggerUiMiddleware));

    public static void UseSwaggerUiMiddleware(this IApplicationBuilder app, IApiVersionDescriptionProvider provider, Func<Stream> streamHtml)
    {
        if (app.IsNull())
            throw new ArgumentNullException(nameof(app));
        var swaggerOptions = App.GetOptions<SwaggerOptions>();
        if (swaggerOptions.Enabled)
        {

            //app.UseSwagger();
            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((doc, item) =>
                {
                    //根据代理服务器提供的协议、地址和路由，生成api文档服务地址
                    doc.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer
                            { Url = $"{item.Scheme}://{item.Host.Value}" }
                    };
                });
            });
            app.UseSwaggerUI(options =>
            {

                // 添加文档信息
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    //这个属性是往SwaggerUI页面head标签中添加我们自己的代码，比如引入一些样式文件，或者执行自己的一些脚本代码
                    //options.HeadContent += $"<script type='text/javascript'>alert('欢迎来到SwaggerUI页面')</script>";

                    //展示默认头部显示的下拉版本信息
                    //options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                    //自由指定头部显示的下拉版本内容
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", ((VersionEnum)description.ApiVersion.MajorVersion).GetDisplayName());
                    //options.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("MiniProfilerSample.index.html");
                    //如果是为空 访问路径就为 根域名/index.html,注意localhost:8001/swagger是访问不到的
                    //options.RoutePrefix = string.Empty;
                    // 如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "swagger"; 则访问路径为 根域名/swagger/index.html

                    //{
                    //    url = "/swagger/logo.png" // 添加 logo
                    //};
                }
                ;
                options.RoutePrefix = swaggerOptions.Route;

                var stream = streamHtml?.Invoke();
                if (stream == null)
                {
                    const string msg = "index.html属性错误";
                    Logger.Error(msg);
                    throw new Exception(msg);
                }
                options.IndexStream = streamHtml;
                options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            });
        }
    }
}
