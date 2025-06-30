using System;
using BearPlatform.Common.Model;
using SqlSugar;

namespace BearPlatform.Entity.Base
{
    /// <summary>
    /// 实体基类 无数据权限检查
    /// </summary>
    [SugarIndex("index_{table}_CreateBy", nameof(CreateBy), OrderByType.Asc)]
    [SugarIndex("index_{table}_IsDeleted", nameof(IsDeleted), OrderByType.Asc)]
    public class BaseEntityNoDataScope<TKey> : RootKey<TKey>, ISoftDeletedEntity where TKey : IEquatable<TKey>
    {

        /// <summary>
        /// 创建者名称
        /// </summary>
        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true)]
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsNullable = true, IsOnlyIgnoreUpdate = true)]
        public long CreateTime { get; set; }

        /// <summary>
        /// 更新者名称
        /// </summary>
        [SugarColumn(IsNullable = true, IsOnlyIgnoreInsert = true)]
        public string UpdateBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [SugarColumn(IsNullable = true, IsOnlyIgnoreInsert = true)]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
