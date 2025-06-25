using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Asp.Versioning;
using Bear.Core.Api.Controllers.Base;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Helper;
using Bear.Core.Common.Model;
using Bear.Core.IBusiness.Permission;
using Bear.Core.Models.Dto.Core.Permission;
using Bear.Core.Models.Dto.Permission;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.Permission;
using Bear.Core.SqlSugar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Crypto;

namespace Bear.Core.Api.Controllers.Permission;


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
