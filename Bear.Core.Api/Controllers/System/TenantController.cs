using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Asp.Versioning;
using Bear.Core.Api.Controllers.Base;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Helper;
using Bear.Core.Common.Model;
using Bear.Core.IBusiness.System;
using Bear.Core.Models;
using Bear.Core.Models.Dto.Core.System;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.System;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel.Core.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bear.Core.Api.Controllers.System;


/// <summary>
/// 租户
/// </summary>
[Route("/api/[controller]/[action]")]
public class TenantController(ITenantService service) : BaseApiController
{
    #region 字段

    private readonly ITenantService _service = service ?? throw new ArgumentNullException(nameof(service));
    #endregion

    #region 内部接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<PagedResults<TenantDTO>> GetPageAsync([FromQuery] TenantParam param) => await _service.GetPageAsync(param);
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<TenantInfo> GetInfoAsync(long id)=> await _service.GetInfoAsync(id);

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<long> AddAsync(UpdateTenantParam param) => await _service.AddAsync(param);

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPut]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<long> UpdateAsync(UpdateTenantParam param) => await _service.UpdateAsync(param);
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<int> Delete([FromBody] HashSet<long> ids) => await _service.DeleteAsync(ids);
    #endregion
}
