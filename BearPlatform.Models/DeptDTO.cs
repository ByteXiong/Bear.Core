using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Entity;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;
using Newtonsoft.Json;
using SqlSugar;

namespace BearPlatform.Models.Dto.Permission;



public class DeptParam :PageParam{
    
    //public long? Id { get; set; }

}
/// <summary>
/// 组织,我的下级
/// </summary>
[AutoMapping(typeof(Dept), typeof(DeptDTO))]
public class DeptDTO : BaseEntityDTO<long>
{
    /// <summary>
    /// 部门名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 父级部门ID
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// 子节点个数
    /// </summary>
    public int SubCount { get; set; }

    /// <summary>
    /// 子节点
    /// </summary>
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<DeptDTO> Children { get; set; }
}


[AutoMapping(typeof(ApisInfo), typeof(Apis))]
public class DeptInfo : BaseEntityDTO<long>
{
    /// <summary>
    /// ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }
}
[AutoMapping(typeof(UpdateDeptParam), typeof(Dept))]
public class UpdateDeptParam : Dept { 


}
