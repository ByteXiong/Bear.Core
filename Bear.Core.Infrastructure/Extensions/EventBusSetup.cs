using Autofac;
using Bear.Core.Core;
using Bear.Core.Core.ConfigOptions;
using Bear.Core.EventBus;
using Bear.Core.EventBus.Abstractions;
using Bear.Core.EventBus.EventBusRabbitMQ;
using Bear.Core.Infrastructure.Messaging.Rabbit.EventHandling;
using Bear.Core.Infrastructure.Messaging.Rabbit.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Bear.Core.Infrastructure.Extensions;

/// <summary>
/// 事件总线启动器
/// </summary>
public static class EventBusSetup
{
    public static void AddEventBusSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var eventBusOptions = App.GetOptions<EventBusOptions>();
        if (eventBusOptions.Enabled)
        {
            var subscriptionClientName = eventBusOptions.SubscriptionClientName;

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.AddTransient<UserQueryIntegrationEventHandler>();

            if (App.GetOptions<MiddlewareOptions>().RabbitMq.Enabled)
            {
                services.AddSingleton<IEventBus, EventBusRabbitMq>(sp =>
                {
                    var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
                    var retryCount = App.GetOptions<RabbitOptions>().RetryCount;

                    return new EventBusRabbitMq(sp, rabbitMqPersistentConnection, iLifetimeScope,
                        subscriptionClientName, eventBusSubcriptionsManager, retryCount);
                });
            }
        }
    }


    public static void ConfigureEventBus(this IApplicationBuilder app)
    {
        var eventBusOptions = App.GetOptions<EventBusOptions>();
        if (eventBusOptions.Enabled)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            eventBus.Subscribe<UserQueryIntegrationEvent, UserQueryIntegrationEventHandler>();
        }
    }
}
