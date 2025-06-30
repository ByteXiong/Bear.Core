using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Permission.User;
using BearPlatform.Models.Base;
using BearPlatform.Models.Dto.Core.Permission.User;
using BearPlatform.Models.Dto.Permission;
using BearPlatform.Models.Permission;
using BearPlatform.SqlSugar;

namespace BearPlatform.Models;

/// <summary>
/// 搜索项
/// </summary>
public class UserParam  : PageParam{
    /// <summary>
    /// 关键字
    /// </summary>
    public string KeyWord { get; set; }

}
/// <summary>
/// 用户Dto
/// </summary>
[AutoMapping(typeof(User), typeof(UserDTO))]
public class UserDTO : BaseEntityDTO<long>
{
    /// <summary>
    /// 用户名
    /// </summary>  
    public string UserName { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string NickName { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// 内置管理员
    /// </summary>
    public bool IsAdmin { get; set; }

    /// <summary>
    /// 是否激活
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public long DeptId { get; set; }

    /// <summary>
    /// 电话
    /// </summary>
    public string Phone { get; set; }


    /// <summary>
    /// 头像文件路径
    /// </summary>
    public string AvatarPath { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    public string Gender { get; set; }

    /// <summary>
    /// 最后修改密码时间
    /// </summary>
    public DateTime? PasswordReSetTime { get; set; }

    ///// <summary>
    ///// 角色列表
    ///// </summary>
    //public List<RoleSmallDto> Roles { get; set; }

    ///// <summary>
    ///// 部门
    ///// </summary>
    //public DeptSmallDto Dept { get; set; }

    ///// <summary>
    ///// 岗位列表
    ///// </summary>
    //public List<JobSmallDto> Jobs { get; set; }

    /// <summary>
    /// 租户ID
    /// </summary>
    public int TenantId { get; set; }

    //    /// <summary>
    //    /// 租户
    //    /// </summary>
    //    public TenantDTO Tenant { get; set; }
}


/// <summary>
/// 详情
/// </summary>
[AutoMapping(typeof(User), typeof(UserInfo))]
    public class UserInfo : BaseEntityDTO<long>
    {
        /// <summary>
        /// 
        /// </summary>
        public UserInfo()
        {
            Roles = new List<RoleInfo>();
        //Jobs = new List<JobSmallDto>();
    }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 内置管理员
        /// </summary>
        public bool IsAdmin { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public long DeptId { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }


        /// <summary>
        /// 头像文件路径
        /// </summary>
        public string AvatarPath { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// 最后修改密码时间
        /// </summary>
        public DateTime? PasswordReSetTime { get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    public List<RoleInfo> Roles { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    public DeptInfo Dept { get; set; }

        ///// <summary>
        ///// 岗位列表
        ///// </summary>
        //public List<JobSmallDto> Jobs { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        public int TenantId { get; set; }

        ///// <summary>
        ///// 租户
        ///// </summary>
        //public TenantVo Tenant { get; set; }
    }
    /// <summary>
    ///  用户修改
    /// </summary>
    [AutoMapping(typeof(User), typeof(UpdateUserParam))]
    public class UpdateUserParam : BaseEntityDTO<long>
    {

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        public string NickName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        [Required]
        [RegularExpression(@"^(13[0-9]|14[01456879]|15[0-35-9]|16[2567]|17[0-8]|18[0-9]|19[0-35-9])\d{8}$")]
        public string Phone { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [Required]
        public string Gender { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        [Required]
        public UserDeptDto Dept { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [Required]
        public List<UserRoleDto> Roles { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        [Required]
        public List<UserJobDto> Jobs { get; set; }

        /// <summary>
        /// 租户ID
        /// </summary>
        public int? TenantId { get; set; }
    }

