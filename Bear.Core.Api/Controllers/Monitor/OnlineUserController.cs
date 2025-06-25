using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Bear.Core.Api.Controllers.Base;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Helper;
using Bear.Core.Common.Model;
using Bear.Core.Common.WebApp;
using Bear.Core.IBusiness.Monitor;
using Bear.Core.Models.Queries.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bear.Core.Api.Controllers.Monitor;

/// <summary>
/// 在线用户
/// </summary>
[Route("/api/[controller]/[action]")]
public class OnlineUserController : BaseApiController
{
    private readonly IOnlineUserService _onlineUserService;

    public OnlineUserController(IOnlineUserService onlineUserService)
    {
        _onlineUserService = onlineUserService;
    }

    #region 对内接口

    /// <summary>
    /// 在线用户列表
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("Sys.Query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<LoginUserInfo>))]
    public async Task<ActionResult> Query(Pagination pagination)
    {
        var onlineUserList = await _onlineUserService.QueryAsync(pagination);

        return JsonContent(onlineUserList, pagination);
    }

    /// <summary>
    /// 强制登出用户
    /// </summary>
    /// <param name="idCollection"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("out")]
    [Description("强退用户")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm))]
    public async Task<ActionResult> DropOut([FromBody] IdCollectionString idCollection)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        await _onlineUserService.DropOutAsync(idCollection.IdArray);

        return Ok(OperateResult.Success());
    }

    /// <summary>
    /// 导出在线用户
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Description("Sys.Export")]
    [Route("download")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    public async Task<ActionResult> Download()
    {
        var appSecretExports = await _onlineUserService.DownloadAsync();
        var data = new ExcelHelper().GenerateExcel(appSecretExports, out var mimeType, out var fileName);
        return new FileContentResult(data, mimeType)
        {
            FileDownloadName = fileName
        };
    }

    #endregion
}
