using BearPlatform.Common.Attributes;

namespace BearPlatform.Core.ConfigOptions;

/// <summary>
/// 系统配置
/// </summary>
[OptionsSettings]
public class SystemOptions
{
    /// <summary>
    /// 是否初始化表
    /// </summary>
    public bool IsInitTable { get; set; }

    /// <summary>
    /// 是否初始化数据
    /// </summary>
    public bool IsInitData { get; set; }

    /// <summary>
    /// 是否开启读写分离
    /// </summary>
    public bool IsCqrs { get; set; }

    /// <summary>
    /// 是否开发模式
    /// </summary>
    public bool IsQuickDebug { get; set; }

    /// <summary>
    /// 用户默认密码
    /// </summary>
    public string UserDefaultPassword { get; set; }

    /// <summary>
    /// 文件限制大小
    /// </summary>
    public int FileLimitSize { get; set; }

    /// <summary>
    /// Hmac签名密钥
    /// </summary>
    public string HmacSecret { get; set; }

    /// <summary>
    /// 主库
    /// </summary>
    public string MasterDataBase { get; set; }

    /// <summary>
    /// 日志库
    /// </summary>
    public string LogDataBase { get; set; }

    /// <summary>
    /// 是否使用Redis缓存
    /// </summary>
    public bool UseRedisCache { get; set; }
}
