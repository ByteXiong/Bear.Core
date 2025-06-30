using BearPlatform.Common.Extensions;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using SqlSugar;

namespace BearPlatform.Core.Caches.SqlSugar;

/// <summary>
/// Distributed缓存
/// 实现SqlSugar.ICacheService
/// </summary>
public class SqlSugarDistributedCache : ICacheService
{
    private readonly Lazy<IDistributedCache> _caching = new(App.GetService<IDistributedCache>());
    private IDistributedCache Caching => _caching.Value;


    public void Add<TV>(string key, TV value)
    {
        var valStr = value.ToJson();
        Caching.SetString(key, valStr);
    }

    public void Add<TV>(string key, TV value, int cacheDurationInSeconds)
    {
        var valStr = value.ToJson();
        DistributedCacheEntryOptions op = new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDurationInSeconds),
        };
        Caching.SetString(key, valStr, op);
    }

    public bool ContainsKey<TV>(string key)
    {
        return Caching.Get(key) != null;
    }

    public TV Get<TV>(string key)
    {
        var val = Caching.GetString(key);
        if (val == null) return default(TV);
        return JsonConvert.DeserializeObject<TV>(val);
    }

    public IEnumerable<string> GetAllKey<TV>()
    {
        //throw new NotImplementedException("IDistributedCache不支持模糊查询KEY");
        //实现思路 创建一个缓存区。然后添加缓存时，把key增加到这个缓存区
        //每次获取时都需要检查一下缓存是否存在 不存在则删除
        //这个获取所有key 获取这个定义得缓存区就行了 
        return new[] { "" };
    }

    public TV GetOrCreate<TV>(string cacheKey, Func<TV> create, int cacheDurationInSeconds = int.MaxValue)
    {
        if (ContainsKey<TV>(cacheKey))
        {
            return Get<TV>(cacheKey);
        }

        TV val = create();
        Add(cacheKey, val, cacheDurationInSeconds);
        return val;
    }

    public void Remove<TV>(string key)
    {
        Caching.Remove(key);
    }
}
