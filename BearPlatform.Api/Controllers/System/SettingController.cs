using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using BearPlatform.Common.Model;
using BearPlatform.IBusiness.System;
using BearPlatform.Models.Dto.Core.System;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.ViewModel.Core.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers.System;

/// <summary>
/// 全局设置管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class SettingController : BaseApiController
{
    #region 字段

    private readonly ISettingService _settingService;

    #endregion

    #region 构造函数

    public SettingController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    #endregion

    #region 内部接口

    /// <summary>
    /// 新增设置
    /// </summary>
    /// <param name="createUpdateSettingDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    [Description("Sys.Create")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActionResultVm))]
    public async Task<ActionResult> Create(
        [FromBody] CreateUpdateSettingDto createUpdateSettingDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _settingService.CreateAsync(createUpdateSettingDto);
        return Ok(result);
    }

    /// <summary>
    /// 更新设置
    /// </summary>
    /// <param name="createUpdateSettingDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("edit")]
    [Description("Sys.Edit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update(
        [FromBody] CreateUpdateSettingDto createUpdateSettingDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _settingService.UpdateAsync(createUpdateSettingDto);
        return Ok(result);
    }

    /// <summary>
    /// 删除设置
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

        var result = await _settingService.DeleteAsync(idCollection.IdArray);
        return Ok(result);
    }

    /// <summary>
    /// 获取设置列表
    /// </summary>
    /// <param name="settingQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("Sys.Query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm<List<SettingVo>>))]
    public async Task<ActionResult> Query(SettingQueryCriteria settingQueryCriteria, Pagination pagination)
    {
        var settingList = await _settingService.QueryAsync(settingQueryCriteria, pagination);

        return JsonContent(settingList, pagination);
    }


    /// <summary>
    /// 导出设置
    /// </summary>
    /// <param name="settingQueryCriteria"></param>
    /// <returns></returns>
    [HttpGet]
    [Description("Sys.Export")]
    [Route("download")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    public async Task<ActionResult> Download(SettingQueryCriteria settingQueryCriteria)
    {
        var settingExports = await _settingService.DownloadAsync(settingQueryCriteria);
        var data = new ExcelHelper().GenerateExcel(settingExports, out var mimeType, out var fileName);
        return new FileContentResult(data, mimeType)
        {
            FileDownloadName = fileName
        };
    }

    #endregion
}
