using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.IBusiness.System;
using BearPlatform.Models.Dto.Core.System;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.ViewModel.Core.System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers.System;

/// <summary>
/// 文件存储管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class FileRecordController : BaseApiController
{
    #region 字段

    private readonly IFileRecordService _fileRecordService;

    #endregion

    #region 构造函数

    public FileRecordController(IFileRecordService fileRecordService)
    {
        _fileRecordService = fileRecordService;
    }

    #endregion

    #region 内部接口

    /// <summary>
    /// 新增文件
    /// </summary>
    /// <param name="file"></param>
    /// <param name="createUpdateFileRecordDto"></param>
    /// <returns></returns>
    [HttpPost, HttpOptions]
    [Route("upload")]
    [Description("Sys.Create")]
    public async Task<ActionResult> Upload(CreateUpdateFileRecordDto createUpdateFileRecordDto,
        [FromForm] IFormFile file)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        if (file.IsNull())
        {
            return Error(App.L.R("{0}required", "file"));
        }


        var fileLimitSize = App.GetOptions<SystemOptions>().FileLimitSize * 1024 * 1024;
        if (file.Length > fileLimitSize)
        {
            return Error(App.L.R("Error.FileTooLarge{0}", fileLimitSize));
        }

        var result = await _fileRecordService.CreateAsync(createUpdateFileRecordDto, file);
        return Ok(result);
    }

    /// <summary>
    /// 更新文件描述
    /// </summary>
    /// <param name="createUpdateAppSecretDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("edit")]
    [Description("Sys.Edit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update(
        [FromBody] CreateUpdateFileRecordDto createUpdateAppSecretDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        var result = await _fileRecordService.UpdateAsync(createUpdateAppSecretDto);
        return Ok(result);
    }

    /// <summary>
    /// 删除文件
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

        var result = await _fileRecordService.DeleteAsync(idCollection.IdArray);
        return Ok(result);
    }


    /// <summary>
    /// 获取文件列表
    /// </summary>
    /// <param name="fileRecordQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("Sys.Query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm<List<FileRecordVo>>))]
    public async Task<ActionResult> Query(FileRecordQueryCriteria fileRecordQueryCriteria,
        Pagination pagination)
    {
        var fileRecordList = await _fileRecordService.QueryAsync(fileRecordQueryCriteria, pagination);

        return JsonContent(fileRecordList, pagination);
    }


    /// <summary>
    /// 导出文件记录
    /// </summary>
    /// <param name="fileRecordQueryCriteria"></param>
    /// <returns></returns>
    [HttpGet]
    [Description("Sys.Export")]
    [Route("download")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    public async Task<ActionResult> Download(FileRecordQueryCriteria fileRecordQueryCriteria)
    {
        var fileRecordExports = await _fileRecordService.DownloadAsync(fileRecordQueryCriteria);
        var data = new ExcelHelper().GenerateExcel(fileRecordExports, out var mimeType, out var fileName);
        return new FileContentResult(data, mimeType)
        {
            FileDownloadName = fileName
        };
    }

    #endregion
}
