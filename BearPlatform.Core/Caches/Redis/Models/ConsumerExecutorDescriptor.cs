using System.Reflection;
using BearPlatform.Common.Attributes.Redis;

namespace BearPlatform.Core.Caches.Redis.Models;

public class ConsumerExecutorDescriptor
{
    public MethodInfo MethodInfo { get; set; }

    public TypeInfo ImplTypeInfo { get; set; }

    public TopicAttribute Attribute { get; set; }
}
