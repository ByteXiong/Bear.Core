using System.Reflection;
using Bear.Core.Common.Attributes.Redis;

namespace Bear.Core.Core.Caches.Redis.Models;

public class ConsumerExecutorDescriptor
{
    public MethodInfo MethodInfo { get; set; }

    public TypeInfo ImplTypeInfo { get; set; }

    public TopicAttribute Attribute { get; set; }
}
