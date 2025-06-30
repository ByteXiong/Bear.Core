using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Entity;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Entity.Core.Permission.Role;
using BearPlatform.Models.Base;
using BearPlatform.Models.Dto.Core.Permission.Role;
using BearPlatform.Models.Dto.Permission;
using BearPlatform.SqlSugar;
using Newtonsoft.Json;

namespace BearPlatform.Models.Permission;

/// <summary>
///  角色分页查询
/// </summary>
public class RoleParam : PageParam
{
    /// <summary>
    /// 关键字
    /// </summary>
    public string KeyWord { get; set; }
}
/// <summary>
/// 角色Dto
/// </summary>
[AutoMapping(typeof(Role), typeof(RoleDTO))]
public class RoleDTO : BaseEntityDTO<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 数据权限
    /// </summary>
    public DataScopeType DataScopeType { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    public string Permission { get; set; }

    /// <summary>
    /// 菜单列表
    /// </summary>
    [JsonProperty(PropertyName = "menus")]
    public List<RouteDTO> MenuList { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    [JsonProperty(PropertyName = "depts")]
    public List<DeptDTO> DeptList { get; set; }

    /// <summary>
    /// 菜单列表
    /// </summary>
    [JsonProperty(PropertyName = "apis")]
    public List<Apis> Apis { get; set; }
}

/// <summary>
/// 详情
/// </summary>
[AutoMapping(typeof(Role), typeof(RoleInfo))]
public class RoleInfo : BaseEntityDTO<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 数据权限
    /// </summary>
    public DataScopeType DataScopeType { get; set; }

    /// <summary>
    /// 权限标识
    /// </summary>
    public string Permission { get; set; }

    /// <summary>
    /// 菜单列表
    /// </summary>
    [JsonProperty(PropertyName = "menus")]
    public List<MenuInfo> Menus { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    [JsonProperty(PropertyName = "depts")]
    public List<DeptInfo> Depts { get; set; }

    /// <summary>
    /// 菜单列表
    /// </summary>
    [JsonProperty(PropertyName = "apis")]
    public List<Apis> Apis { get; set; }
}
/// <summary>
/// 角色Dto
/// </summary>
[AutoMapping(typeof(Role), typeof(UpdateRoleParam))]
public class UpdateRoleParam : BaseEntityDTO<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// 数据权限
    /// </summary>
    public DataScopeType DataScopeType { get; set; }

    /// <summary>
    /// 标识
    /// </summary>
    [Required]
    public string Permission { get; set; }

    /// <summary>
    /// 角色部门
    /// </summary>
    public List<RoleDeptDto> Depts { get; set; }

    /// <summary>
    /// 角色菜单
    /// </summary>
    public List<RoleMenuDto> Menus { get; set; }

    /// <summary>
    /// 角色菜单
    /// </summary>
    public List<RoleApisDto> Apis { get; set; }
}


public class RolePermissionParam
{
    public long RoleId { get; set; }

    public List<long> MenuIds { get; set; }
    public List<Guid> Def { get; set; }
    public List<Guid> Pc { get; set; }
    public List<Guid> App { get; set; }
}
