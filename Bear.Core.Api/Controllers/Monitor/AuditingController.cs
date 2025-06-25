using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Bear.Core.Api.Controllers.Base;
using Bear.Core.Common.Attributes;
using Bear.Core.Common.Model;
using Bear.Core.IBusiness.Monitor;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.System;
using Bear.Core.ViewModel.Core.Monitor;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bear.Core.Api.Controllers.Monitor;

/// <summary>
/// 审计日志管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class AuditingController : BaseApiController
{
    #region 字段

    private readonly IAuditLogService _auditInfoService;

    #endregion

    #region 构造函数

    public AuditingController(IAuditLogService auditInfoService)
    {
        _auditInfoService = auditInfoService;
    }

    #endregion

    #region 内部接口

    /// <summary>
    /// 审计列表
    /// </summary>
    /// <param name="logQueryCriteria">查询对象</param>
    /// <param name="pagination">分页对象</param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("Sys.Query")]
    [NotAudit]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm<List<AuditLogVo>>))]
    public async Task<ActionResult> Query(LogQueryCriteria logQueryCriteria,
        Pagination pagination)
    {
        var auditInfos = await _auditInfoService.QueryAsync(logQueryCriteria, pagination);

        return JsonContent(auditInfos, pagination);
    }


    /// <summary>
    /// 当前用户行为
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("current")]
    [Description("Action.UserConduct")]
    [NotAudit]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm<List<AuditLogVo>>))]
    public async Task<ActionResult> FindListByCurrent(Pagination pagination)
    {
        var auditInfos = await _auditInfoService.QueryByCurrentAsync(pagination);

        return JsonContent(auditInfos, pagination);
    }

    #endregion
}
