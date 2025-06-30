using RabbitMQ.Client;

namespace BearPlatform.EventBus.EventBusRabbitMQ;

/// <summary>
/// RabbitMQ 持久连接
/// </summary>
public interface IRabbitMqPersistentConnection
{
    /// <summary>
    /// 已连接
    /// </summary>
    bool IsConnected { get; }

    /// <summary>
    /// 尝试连接
    /// </summary>
    /// <returns></returns>
    bool TryConnect();

    IModel CreateModel();
}
