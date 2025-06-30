using System.Collections.Generic;
using BearPlatform.Common.Enums;
using BearPlatform.Entity.Base;
using BearPlatform.Entity.Core.Permission.User;
using BearPlatform.Entity.Permission;
using SqlSugar;

namespace BearPlatform.Entity.Core.Permission.Role
{
    /// <summary>
    /// 角色
    /// </summary>
    [SugarTable("sys_role")]
    public class Role : BaseEntity<long>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 角色等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Description { get; set; }

        /// <summary>
        /// 数据权限
        /// </summary>
        public DataScopeType DataScopeType { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        [SugarColumn(Length = 20)]
        public string Permission { get; set; }

        /// <summary>
        /// 菜单集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(RoleMenu), nameof(RoleMenu.RoleId), nameof(RoleMenu.MenuId))]
        public List<Menu> Menus { get; set; }

        /// <summary>
        /// 部门集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(RoleDept), nameof(RoleDept.RoleId), nameof(RoleDept.DeptId))]
        public List<Dept> Depts { get; set; }

        /// <summary>
        /// 用户列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(UserRole), nameof(UserRole.RoleId), nameof(UserRole.UserId))]
        public List<User.User> Users { get; set; }


        /// <summary>
        /// 菜单集合
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(RoleApis), nameof(RoleApis.RoleId), nameof(RoleApis.ApisId))]
        public List<Apis> Apis { get; set; }
    }
}
