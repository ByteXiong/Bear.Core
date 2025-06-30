using System.Threading.Tasks;
using BearPlatform.ViewModel.ServerInfo;

namespace BearPlatform.IBusiness.Monitor;

/// <summary>
/// 服务器资源信息接口
/// </summary>
public interface IServerResourcesService
{
    /// <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    Task<ServerResourcesInfo> Query();
}
