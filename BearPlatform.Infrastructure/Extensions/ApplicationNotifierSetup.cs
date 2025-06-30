using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using Microsoft.AspNetCore.Builder;

namespace BearPlatform.Infrastructure.Extensions;

public static class ApplicationNotifierSetup
{
    public static void ApplicationStartedNotifier(this WebApplication app)
    {
        if (app.IsNull())
            throw new ArgumentNullException(nameof(app));
        app.Lifetime.ApplicationStarted.Register(() =>
        {
            var port = "8002";
            if (app.Configuration["urls"] != null)
            {
                port = app.Configuration["urls"].Split(':').Last();
            }
            ConsoleHelper.Write($"\t应用程序启动成功! 端口号 : ", ConsoleColor.Green);
            ConsoleHelper.WriteLine(port, ConsoleColor.Red);

            ConsoleHelper.Write("\t框架底层使用的是apevolo的框架", ConsoleColor.Green);
            ConsoleHelper.Write("http://doc.apevolo.com/", ConsoleColor.Red);
            ConsoleHelper.WriteLine("欢迎大家订阅支持", ConsoleColor.Green);
            ConsoleHelper.WriteLine("\t会逐步完善属于自己独特的框架风格", ConsoleColor.Green);
            ConsoleHelper.WriteLine("\tBearPlatform, 目前生态还在建设中,欢迎大家提出问题,作者(Wx: Byte_Xiong)会再接再厉", ConsoleColor.Green);
            ConsoleHelper.WriteLine("\tBearPlatform 不仅仅是一个.NET后端框架", ConsoleColor.Green);
            ConsoleHelper.WriteLine("\tSwagger+ alovaJs+Vue3+TS重新定义前后端开发", ConsoleColor.Green);
            ConsoleHelper.WriteLine();
            ConsoleHelper.WriteLine("\t前后端 运行起来写写 会有意想不到的彩蛋", ConsoleColor.Red);
            ConsoleHelper.WriteLine("\t我们不止于此...", ConsoleColor.Red);
        });
    }
}
