using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;
using SqlSugar;

namespace BearPlatform.Models;


/// <summary>
/// 岗位
/// </summary>
#region 查询参数
public class JobParam : PageParam
{

}
#endregion

#region DTO
/// <summary>
///  分页
/// </summary>
[AutoMapping(typeof(JobDTO), typeof(Job))]
public class JobDTO : RootKeyDTO<long>
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// 编号
    /// </summary>
    public string Code { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否激活
    /// </summary>
    [SugarColumn(IsNullable = false)]
    public bool Enabled { get; set; }

}

/// <summary>
/// 详情
/// </summary>
[AutoMapping(typeof(JobInfo), typeof(Job))]
public class JobInfo : Job
{

}
/// <summary>
/// 更新
/// </summary>
[AutoMapping(typeof(UpdateJobParam), typeof(Job))]
public class UpdateJobParam : Job
{
}
#endregion 
/// <summary>
/// 
/// </summary>
[AutoMapping(typeof(Job), typeof(JobSelectDTO))]
public class JobSelectDTO
{
}
