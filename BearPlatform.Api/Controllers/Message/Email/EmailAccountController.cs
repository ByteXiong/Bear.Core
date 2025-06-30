using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using BearPlatform.Common.Model;
using BearPlatform.IBusiness.Message.Email;
using BearPlatform.Models.Dto.Core.Message.Email;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Message;
using BearPlatform.ViewModel.Core.Message.Email;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers.Message.Email;

/// <summary>
/// 邮箱账户管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class EmailAccountController : BaseApiController
{
    private readonly IEmailAccountService _emailAccountService;

    public EmailAccountController(IEmailAccountService emailAccountService)
    {
        _emailAccountService = emailAccountService;
    }


    /// <summary>
    /// 新增邮箱账户
    /// </summary>
    /// <param name="createUpdateEmailAccountDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    [Description("Sys.Create")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActionResultVm))]
    public async Task<ActionResult> Create(
        [FromBody] CreateUpdateEmailAccountDto createUpdateEmailAccountDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _emailAccountService.CreateAsync(createUpdateEmailAccountDto);
        return Ok(result);
    }

    /// <summary>
    /// 更新邮箱账户
    /// </summary>
    /// <param name="createUpdateEmailAccountDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("edit")]
    [Description("Sys.Edit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update(
        [FromBody] CreateUpdateEmailAccountDto createUpdateEmailAccountDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _emailAccountService.UpdateAsync(createUpdateEmailAccountDto);
        return Ok(result);
    }

    /// <summary>
    /// 删除邮箱账户
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

        var result = await _emailAccountService.DeleteAsync(idCollection.IdArray);
        return Ok(result);
    }

    /// <summary>
    /// 邮箱账户列表
    /// </summary>
    /// <param name="emailAccountQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("Sys.Query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm<List<EmailAccountVo>>))]
    public async Task<ActionResult> Query(EmailAccountQueryCriteria emailAccountQueryCriteria,
        Pagination pagination)
    {
        var emailAccountList = await _emailAccountService.QueryAsync(emailAccountQueryCriteria, pagination);


        return JsonContent(emailAccountList, pagination);
    }

    /// <summary>
    /// 导出邮箱账户
    /// </summary>
    /// <param name="emailAccountQueryCriteria"></param>
    /// <returns></returns>
    [HttpGet]
    [Description("Sys.Export")]
    [Route("download")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    public async Task<ActionResult> Download(EmailAccountQueryCriteria emailAccountQueryCriteria)
    {
        var emailAccountExports = await _emailAccountService.DownloadAsync(emailAccountQueryCriteria);
        var data = new ExcelHelper().GenerateExcel(emailAccountExports, out var mimeType, out var fileName);
        return new FileContentResult(data, mimeType)
        {
            FileDownloadName = fileName
        };
    }
}
