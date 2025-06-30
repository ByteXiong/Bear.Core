using System.Collections.Generic;
using BearPlatform.Entity.Base;
using BearPlatform.Entity.Core.Permission.Role;
using SqlSugar;

namespace BearPlatform.Entity.Core.Permission
{
    /// <summary>
    /// 部门
    /// </summary>
    [SugarTable("sys_dept")]
    public class Dept : BaseEntity<long>
    {
        /// <summary>
        /// 部门名称
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string Name { get; set; }

        /// <summary>
        /// 父级部门ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public long ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int Sort { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool Enabled { get; set; }

        /// <summary>
        /// 子节点个数
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int SubCount { get; set; }


        /// <summary>
        /// 用户列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(User.User.DeptId), nameof(Id))]
        public List<User.User> Users { get; set; }

        /// <summary>
        /// 用户集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(RoleDept), nameof(RoleDept.DeptId), nameof(RoleDept.RoleId))]
        public List<Role.Role> Roles { get; set; }
    }
}
