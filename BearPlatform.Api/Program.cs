using System;
using System.Reflection;
using Asp.Versioning.ApiExplorer;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using BearPlatform.Common.ClassLibrary;
using BearPlatform.Common.Global;
using BearPlatform.Common.IdGenerator;
using BearPlatform.Common.IdGenerator.Contract;
using BearPlatform.Common.MultiLanguage.Resources;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.Core.Internal;
using BearPlatform.Core.Mapping;
using BearPlatform.Infrastructure.ActionFilter;
using BearPlatform.Infrastructure.Extensions;
using BearPlatform.Infrastructure.Middleware;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//配置雪花ID方法参数
IdHelper.SetIdGeneratorOptions(new IdGeneratorOptions(1));

// 配置容器
builder.Host
    .UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        hostingContext.Configuration.ConfigureApplication();
        config.Sources.Clear();
        config.AddJsonFile(builder.Environment.IsDevelopment() ? "appsettings.Development.json" : "appsettings.json",
                optional: true, reloadOnChange: false)
            .AddJsonFile("IpRateLimit.json", optional: true, reloadOnChange: false);
    }).UseSerilogMiddleware()
    .ConfigureContainer<ContainerBuilder>(b => { b.RegisterModule(new AutofacRegister()); });
builder.ConfigureApplication();


// 配置服务
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddSingleton<IRegister, CustomMapper>();
builder.Services.AddSingleton<IMapper, Mapper>();
builder.Services.AddSingleton(new AppSettings(builder.Configuration, builder.Environment));
builder.Services.AddOptionRegisterSetup();
builder.Services.AddCustomMultiLanguages();
// builder.Services.Configure<Configs>(configuration);
// var configs = configuration.Get<Configs>();
builder.Services.AddSerilogSetup();
builder.Services.Configure<KestrelServerOptions>(options => { options.AllowSynchronousIO = true; });
builder.Services.Configure<IISServerOptions>(options => { options.AllowSynchronousIO = true; });
builder.Services.AddCacheSetup();
builder.Services.AddSqlSugarSetup();
builder.Services.AddDbSetup();
builder.Services.AddCorsSetup();
builder.Services.AddMiniProfilerSetup();
builder.Services.AddSwaggerSetup();
builder.Services.AddQuartzNetJobSetup();
builder.Services.AddAuthorizationSetup();
builder.Services.AddBrowserDetection();
builder.Services.AddRedisInitMqSetup();
builder.Services.AddIpStrategyRateLimitSetup();
builder.Services.AddRabbitMqSetup();
builder.Services.AddEventBusSetup();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // 设置会话过期时间
    options.Cookie.HttpOnly = true; // 安全设置，防止客户端脚本访问
    options.Cookie.IsEssential = true; // 确保在没有同意 Cookie 的情况下也能使用
});
builder.Services.AddControllers(options =>
    {
        // 异常过滤器
        options.Filters.Add<ExceptionLogFilter>();
        // 审计过滤器
        //options.Filters.Add<AuditLogFilter>();
    })
    //.AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(typeof(Language))
    .AddControllersAsServices()
    .AddNewtonsoftJson(options =>
        {
            //全局忽略循环引用
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            //options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            //options.SerializerSettings.ContractResolver = new CustomContractResolver();
            //自定义处理
            options.SerializerSettings.ContractResolver = new CustomContractResolver();
        }
    );
builder.Services.AddIpSearcherSetup();

// 配置中间件
var app = builder.Build();

app.ConfigureApplication();
app.ApplicationStartedNotifier();

//实体映射配置
var mapper = app.Services.GetRequiredService<IRegister>();
TypeAdapterConfig.GlobalSettings.Apply(mapper);

//多语言请求扩展
app.UseCustomRequestLocalization();

//IP限流
app.UseIpLimitMiddleware();


//获取远程真实ip,如果不是nginx代理部署可以不要
app.UseMiddleware<RealIpMiddleware>();
//处理访问不存在的接口
//app.UseMiddleware<NotFoundMiddleware>();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.Use(next => context =>
{
    context.Request.EnableBuffering();
    return next(context);
});

//autofac
//AutofacHelper.Container = app.Services.GetAutofacRoot();

app.UseSession();
// // Swagger Auth
app.UseSwaggerAuthorized();
//Swagger UI
var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
app.UseSwaggerUiMiddleware(provider, () => Assembly.GetExecutingAssembly().GetManifestResourceStream("BearPlatform.Api.index.html"));



// CORS跨域
app.UseCors(App.GetOptions<CorsOptions>().Name);
//静态文件
app.UseStaticFiles();
//cookie
app.UseCookiePolicy();
//错误页
app.UseStatusCodePages();
app.UseRouting();
// 认证
app.UseAuthentication();
// 授权
app.UseAuthorization();
//性能监控
app.UseMiniProfilerMiddleware();

//app.UseHttpMethodOverride();

// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllerRoute(
//         name: "default",
//         pattern: "{controller=Home}/{action=Index}/{id?}");
// });

//种子数据
app.UseDataSeederMiddleware();

//作业调度
app.UseQuartzNetJobMiddleware();

//事件总线配置订阅
app.ConfigureEventBus();

// 注册控制器路由
app.MapControllers();

// 运行
app.Run();
