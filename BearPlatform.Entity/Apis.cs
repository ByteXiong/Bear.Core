using System;
using System.Collections.Generic;
using BearPlatform.Entity.Base;
using BearPlatform.Entity.Core.Permission.Role;
using SqlSugar;

namespace BearPlatform.Entity;

/// <summary>
/// 
/// </summary>
[SugarTable("sys_apis")]
public class Apis : BaseEntity<Guid>
{
    /// <summary>
    /// 组
    /// </summary>
    [SugarColumn(IsNullable = false)]
    public string Group { get; set; }

    /// <summary>
    /// 路径
    /// </summary>
    [SugarColumn(IsNullable = false)]
    public string Url { get; set; }

    /// <summary>
    /// 版本
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Description { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    [SugarColumn(IsNullable = false)]
    public string Method { get; set; }

    /// <summary>
    /// 角色集合
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(typeof(RoleApis), nameof(RoleApis.ApisId), nameof(RoleApis.RoleId))]
    public List<Role> Roles { get; set; }

}
