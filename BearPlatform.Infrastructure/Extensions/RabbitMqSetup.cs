using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.EventBus.EventBusRabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace BearPlatform.Infrastructure.Extensions;

/// <summary>
/// rabbitmq启动器
/// </summary>
public static class RabbitMqSetup
{
    public static void AddRabbitMqSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        if (App.GetOptions<MiddlewareOptions>().RabbitMq.Enabled)
        {
            var options = App.GetOptions<RabbitOptions>();
            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var factory = new ConnectionFactory
                {
                    HostName = options.Connection,
                    UserName = options.UserName,
                    Password = options.Password,
                    DispatchConsumersAsync = true
                };
                var retryCount = options.RetryCount;
                return new RabbitMqPersistentConnection(factory, retryCount);
            });
        }
    }
}
