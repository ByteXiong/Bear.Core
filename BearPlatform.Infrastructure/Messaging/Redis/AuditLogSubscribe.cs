using BearPlatform.Common.Attributes.Redis;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using BearPlatform.Core.Caches.Redis.MessageQueue;
using BearPlatform.Entity.Logs;
using BearPlatform.IBusiness.Monitor;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace BearPlatform.Infrastructure.Messaging.Redis;

public class AuditLogSubscribe : IRedisSubscribe
{
    #region Fields

    private readonly ILogger<AuditLogSubscribe> _logger;
    private readonly IAuditLogService _auditInfoService;

    #endregion

    #region Ctor

    public AuditLogSubscribe(IAuditLogService auditLogService, ILogger<AuditLogSubscribe> logger)
    {
        _auditInfoService = auditLogService;
        _logger = logger;
    }

    #endregion

    [SubscribeDelay(MqTopicNameKey.AuditLogQueue, true)]
    private async Task DoSub(List<RedisValue> redisValues)
    {
        try
        {
            if (redisValues.Any())
            {
                List<AuditLog> auditLogs = new List<AuditLog>();
                redisValues.ForEach(x => { auditLogs.Add(x.ToString().ToObject<AuditLog>()); });
                await _auditInfoService.CreateListAsync(auditLogs);
            }
        }
        catch (Exception e)
        {
            _logger.LogCritical(ExceptionHelper.GetExceptionAllMsg(e));
        }
    }
}
