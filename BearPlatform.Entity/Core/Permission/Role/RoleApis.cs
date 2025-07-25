using System;
using SqlSugar;

namespace BearPlatform.Entity.Core.Permission.Role
{
    /// <summary>
    /// 角色Apis关联
    /// </summary>
    [SugarTable("sys_role_apis")]
    public class RoleApis
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// api ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid ApisId { get; set; }
    }
}
