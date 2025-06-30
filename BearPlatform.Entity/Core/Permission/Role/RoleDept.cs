using SqlSugar;

namespace BearPlatform.Entity.Core.Permission.Role
{
    /// <summary>
    /// 角色部门关联
    /// </summary>
    [SugarTable("sys_role_dept")]
    public class RoleDept
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long DeptId { get; set; }
    }
}
