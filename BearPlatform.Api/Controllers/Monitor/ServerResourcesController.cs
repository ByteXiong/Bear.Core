using System.ComponentModel;
using System.Threading.Tasks;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Attributes;
using BearPlatform.IBusiness.Monitor;
using BearPlatform.ViewModel.ServerInfo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers.Monitor;

/// <summary>
/// 服务器管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class ServerResourcesController : BaseApiController
{
    private readonly IServerResourcesService _serverResourcesService;

    public ServerResourcesController(IServerResourcesService serverResourcesService)
    {
        _serverResourcesService = serverResourcesService;
    }

    #region 对内接口

    [HttpGet]
    [Route("resources/info")]
    [Description("Action.ServerResourceInfo")]
    [NotAudit]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ServerResourcesInfo))]
    public async Task<ActionResult> Query()
    {
        var resourcesInfo = await _serverResourcesService.Query();

        return JsonContent(resourcesInfo);
    }

    #endregion
}
