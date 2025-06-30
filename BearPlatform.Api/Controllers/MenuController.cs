using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Attributes;
using BearPlatform.Core;
using BearPlatform.IBusiness.Permission;
using BearPlatform.Models.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers;

/// <summary>
/// 菜单管理
/// </summary>
[Route("/api/[controller]/[action]", Order = 4)]
public class MenuController : BaseApiController
{
    #region 字段

    private readonly IMenuService _service;

    #endregion

    #region 构造函数

    public MenuController(IMenuService service)
    {
        _service = service;
    }

    #endregion

    #region 内部接口



    /// <summary>
    /// 常量路由
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]
    [AllowAnonymous]
    [NotAudit]
    public async Task<List<RouteDTO>> ConstantRoutesAsync() => await _service.ConstantRoutesAsync();


    /// <summary>
    /// 判断路由是否存在
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<bool> IsRouteExistAsync(string name) => await _service.IsRouteExistAsync(name);

    /// <summary>
    /// 列表
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<List<MenuTreeDTO>> GetTreeAsync() => await _service.GetTreeAsync();
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]

    public async Task<MenuInfo> GetInfoAsync(long id) => await _service.GetInfoAsync(id);

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPost]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<long> AddAsync(UpdateMenuParam param) => await _service.AddAsync(param);
    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [HttpPut]
    [ApiVersion("1.0", Deprecated = false)]

    public async Task<long> UpdateAsync(UpdateMenuParam param) => await _service.UpdateAsync(param);



    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<int> DeleteAsync([FromBody] long[] ids) => await _service.DeleteAsync(ids);


    /// <summary>
    /// 菜单下拉
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]
    [AllowAnonymous]
    [NotAudit]
    public async Task<List<RouteTreeSelectDTO>> TreeSelectAsync() => await _service.TreeSelectAsync();
    #endregion

    /// <summary>
    /// 我的菜单
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ApiVersion("1.0", Deprecated = false)]
    [AllowAnonymous]
    [NotAudit]
    public async Task<List<RouteDTO>> MyRoutesAsync() => await _service.BuildTreeAsync(App.HttpUser.Id);




}
