using BearPlatform.Core;
using BearPlatform.Core.Caches.Redis.MessageQueue;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.Infrastructure.Messaging.Redis;
using Microsoft.Extensions.DependencyInjection;

namespace BearPlatform.Infrastructure.Extensions;

/// <summary>
/// redis消息队列
/// </summary>
public static class RedisInitMqSetup
{
    public static void AddRedisInitMqSetup(this IServiceCollection services)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        var systemOptions = App.GetOptions<SystemOptions>();
        var middlewareOptions = App.GetOptions<MiddlewareOptions>();
        var redisOptions = App.GetOptions<RedisOptions>();
        //启动redis消息队列 必须先启动redis缓存
        if (systemOptions.UseRedisCache && middlewareOptions.RedisMq.Enabled)
        {
            services.AddRedisMq(m =>
            {
                //没消息时挂起时长(毫秒)
                m.SuspendTime = redisOptions.SuspendTime;
                //每次消费消息间隔时间(毫秒)
                m.IntervalTime = redisOptions.IntervalTime;
                //如果是批量消费 一次消费最大处理多少条
                m.MaxQueueConsumption = redisOptions.MaxQueueConsumption;
                //显示日志
                m.ShowLog = redisOptions.ShowLog;
                //订阅者类
                m.ListSubscribe = new List<Type>
                {
                    //typeof(EmailRedisSubscribe),
                    typeof(AuditLogSubscribe)
                    //多个类继续往下加
                };
            });
        }
    }
}
