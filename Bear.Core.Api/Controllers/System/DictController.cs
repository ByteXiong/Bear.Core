using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Bear.Core.Api.Controllers.Base;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Helper;
using Bear.Core.Common.Model;
using Bear.Core.IBusiness.System;
using Bear.Core.Models.Dto.Core.System.Dict;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.System;
using Bear.Core.ViewModel.Core.System.Dict;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Bear.Core.Api.Controllers.System;

/// <summary>
/// 字典管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class DictController : BaseApiController
{
    #region 字段

    private readonly IDictService _dictService;

    #endregion

    #region 构造函数

    public DictController(IDictService dictService)
    {
        _dictService = dictService;
    }

    #endregion

    #region 内部接口

    /// <summary>
    /// 新增字典
    /// </summary>
    /// <param name="createUpdateDictDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    [Description("Sys.Create")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActionResultVm))]
    public async Task<ActionResult> Create([FromBody] CreateUpdateDictDto createUpdateDictDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _dictService.CreateAsync(createUpdateDictDto);
        return Ok(result);
    }


    /// <summary>
    /// 更新字典
    /// </summary>
    /// <param name="createUpdateDictDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("edit")]
    [Description("Sys.Edit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update([FromBody] CreateUpdateDictDto createUpdateDictDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _dictService.UpdateAsync(createUpdateDictDto);
        return Ok(result);
    }

    /// <summary>
    /// 删除字典
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

        var result = await _dictService.DeleteAsync(idCollection.IdArray);
        return Ok(result);
    }

    /// <summary>
    /// 查看字典列表
    /// </summary>
    /// <param name="dictQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("Sys.Query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm<List<DictVo>>))]
    public async Task<ActionResult> Query(DictQueryCriteria dictQueryCriteria,
        Pagination pagination)
    {
        var list = await _dictService.QueryAsync(dictQueryCriteria, pagination);

        return JsonContent(list, pagination);
    }

    /// <summary>
    /// 导出字典
    /// </summary>
    /// <param name="dictQueryCriteria"></param>
    /// <returns></returns>
    [HttpGet]
    [Description("Sys.Export")]
    [Route("download")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    public async Task<ActionResult> Download(DictQueryCriteria dictQueryCriteria)
    {
        var dictExports = await _dictService.DownloadAsync(dictQueryCriteria);
        var data = new ExcelHelper().GenerateExcel(dictExports, out var mimeType, out var fileName);
        return new FileContentResult(data, mimeType)
        {
            FileDownloadName = fileName
        };
    }

    #endregion
}
