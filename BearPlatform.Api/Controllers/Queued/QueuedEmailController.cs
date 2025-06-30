using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Model;
using BearPlatform.IBusiness.Queued;
using BearPlatform.Models.Dto.Core.Queued;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Queued;
using BearPlatform.ViewModel.Core.Queued;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers.Queued;

/// <summary>
/// 邮件队列管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class QueuedEmailController : BaseApiController
{
    private readonly IQueuedEmailService _queuedEmailService;

    public QueuedEmailController(IQueuedEmailService queuedEmailService)
    {
        _queuedEmailService = queuedEmailService;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="createUpdateQueuedEmailDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    [Description("Sys.Create")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActionResultVm))]
    public async Task<ActionResult> Create(
        [FromBody] CreateUpdateQueuedEmailDto createUpdateQueuedEmailDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _queuedEmailService.CreateAsync(createUpdateQueuedEmailDto);
        return Ok(result);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="createUpdateQueuedEmailDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("edit")]
    [Description("Sys.Edit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update(
        [FromBody] CreateUpdateQueuedEmailDto createUpdateQueuedEmailDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _queuedEmailService.UpdateAsync(createUpdateQueuedEmailDto);
        return Ok(result);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="idCollection"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("delete")]
    [Description("Sys.Delete")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm))]
    public async Task<ActionResult> Delete([FromBody] IdCollection idCollection)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _queuedEmailService.DeleteAsync(idCollection.IdArray);
        return Ok(result);
    }

    /// <summary>
    /// 列表
    /// </summary>
    /// <param name="queuedEmailQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("Sys.Query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm<List<QueuedEmailVo>>))]
    public async Task<ActionResult> Query(QueuedEmailQueryCriteria queuedEmailQueryCriteria,
        Pagination pagination)
    {
        var queuedEmailDtoList = await _queuedEmailService.QueryAsync(queuedEmailQueryCriteria, pagination);


        return JsonContent(queuedEmailDtoList, pagination);
    }
}
