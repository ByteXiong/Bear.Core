using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Asp.Versioning;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using BearPlatform.Common.Model;
using BearPlatform.IBusiness.Permission;
using BearPlatform.Models.Dto.Core.Permission;
using BearPlatform.Models.Dto.Permission;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Permission;
using BearPlatform.SqlSugar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace BearPlatform.Api.Controllers.Permission;


/// <summary>
/// 部门管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class DeptController(IDeptService service) : BaseApiController
{
    #region 字段

    private readonly IDeptService _service = service ?? throw new ArgumentNullException(nameof(service));
    #endregion

    #region 内部接口
    /// <summary>
    ///  分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<PagedResults<DeptDTO>> GetPageAsync([FromQuery] DeptParam param) => await _service.GetPageAsync(param);
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<DeptInfo> GetInfoAsync(long id) => await _service.GetInfoAsync(id);
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<long> AddAsync(UpdateDeptParam param) => await _service.AddAsync(param);

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPut]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<long> UpdateAsync(UpdateDeptParam param) => await _service.UpdateAsync(param);
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<int> Delete([FromBody] HashSet<long> ids) => await _service.DeleteAsync(ids);
    #endregion

    #region 扩展接口

   
    #endregion

}
