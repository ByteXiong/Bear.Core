using System.Linq.Expressions;
using SqlSugar;

namespace BearPlatform.Models.Queries.Common;

/// <summary>
/// 查询操作对象
/// </summary>
/// <typeparam name="T"></typeparam>
public class QueryOptions<T>
{
    /// <summary>
    /// 分页
    /// </summary>
    public Pagination Pagination { get; set; }

    /// <summary>
    /// 条件表达式
    /// </summary>
    public Expression<Func<T, bool>> WhereLambda { get; set; } = null;


    /// <summary>
    /// 条件模型
    /// </summary>
    public List<IConditionalModel> ConditionalModels { get; set; } = null;


    /// <summary>
    /// 查询表达式
    /// </summary>
    public Expression<Func<T, T>> SelectExpression { get; set; } = null;

    /// <summary>
    /// 是否分表
    /// </summary>
    public bool IsSplitTable { get; set; } = false;

    /// <summary>
    /// 是否查询全部一级导航属性
    /// </summary>
    public bool IsIncludes { get; set; } = false;

    /// <summary>
    /// 忽略的查询导航属性 IncludesAll为true才有用
    /// </summary>
    public string[] IgnorePropertyNameList { get; set; }

    /// <summary>
    /// 锁 分页查询默认NoLock
    /// </summary>
    public string LockString { get; set; } = SqlWith.NoLock;

    /// <summary>
    /// 缓存时间(秒)
    /// </summary>
    public int CacheDurationInSeconds { get; set; } = 0;
}
