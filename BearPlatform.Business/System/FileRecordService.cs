using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Helper;
using BearPlatform.Common.IdGenerator;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.Utils;
using BearPlatform.Entity.Core.System;
using BearPlatform.IBusiness.System;
using BearPlatform.Models.Dto.Core.System;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.ViewModel.Core.System;
using BearPlatform.ViewModel.Report.System;
using Microsoft.AspNetCore.Http;

namespace BearPlatform.Business.System;

/// <summary>
/// 文件记录服务
/// </summary>
public class FileRecordService : BaseServices<FileRecord>, IFileRecordService
{
    #region 基础方法

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="createUpdateFileRecordDto"></param>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<OperateResult> CreateAsync(CreateUpdateFileRecordDto createUpdateFileRecordDto, IFormFile file)
    {
        var fileExtensionName = FileHelper.GetExtensionName(file.FileName);
        var fileTypeName = FileHelper.GetFileTypeName(fileExtensionName);
        var fileTypeNameEn = FileHelper.GetFileTypeNameEn(fileTypeName);

        string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + IdHelper.NextId() +
                          file.FileName.Substring(Math.Max(file.FileName.LastIndexOf('.'), 0));

        var prefix = App.WebHostEnvironment.WebRootPath;
        string filePath = Path.Combine(prefix, "uploads", "file", fileTypeNameEn);
        if (!Directory.Exists(filePath))
        {
            Directory.CreateDirectory(filePath);
        }

        filePath = Path.Combine(filePath, fileName);
        await using (var fs = new FileStream(filePath, FileMode.CreateNew))
        {
            await file.CopyToAsync(fs);
            fs.Flush();
        }

        string relativePath = Path.GetRelativePath(prefix, filePath);
        relativePath = "/" + relativePath.Replace("\\", "/");
        var fileRecord = new FileRecord
        {
            Description = createUpdateFileRecordDto.Description,
            OriginalName = file.FileName,
            NewName = fileName,
            FilePath = relativePath,
            Size = FileHelper.GetFileSize(file.Length),
            ContentType = file.ContentType,
            ContentTypeName = fileTypeName,
            ContentTypeNameEn = fileTypeNameEn
        };
        var result = await AddAsync(fileRecord);
        return OperateResult.Result(result);
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="createUpdateFileRecordDto"></param>
    /// <returns></returns>
    public async Task<OperateResult> UpdateAsync(CreateUpdateFileRecordDto createUpdateFileRecordDto)
    {
        //取出待更新数据
        var oldFileRecord = await TableWhere(x => x.Id == createUpdateFileRecordDto.Id).FirstAsync();
        if (oldFileRecord.IsNull())
        {
            throw new BusException(ValidationError.NotExist(createUpdateFileRecordDto,
                LanguageKeyConstants.FileRecord,
                nameof(createUpdateFileRecordDto.Id)));
        }

        var fileRecord = App.Mapper.MapTo<FileRecord>(createUpdateFileRecordDto);
        var result = await UpdateAsync(fileRecord);
        return OperateResult.Result(result);
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(HashSet<long> ids)
    {
        var appSecretList = await TableWhere(x => ids.Contains(x.Id)).ToListAsync();
        if (appSecretList.Count == 0)
        {
            throw new BusException(ValidationError.NotExist());
        }

        await LogicDelete<FileRecord>(x => ids.Contains(x.Id));
        foreach (var appSecret in appSecretList)
        {
            FileHelper.Delete(appSecret.FilePath);
        }

        return OperateResult.Success();
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="fileRecordQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public async Task<List<FileRecordVo>> QueryAsync(FileRecordQueryCriteria fileRecordQueryCriteria,
        Pagination pagination)
    {
        var queryOptions = new QueryOptions<FileRecord>
        {
            Pagination = pagination,
            ConditionalModels = fileRecordQueryCriteria.ApplyQueryConditionalModel()
        };
        return App.Mapper.MapTo<List<FileRecordVo>>(
            await TablePageAsync(queryOptions));
    }

    /// <summary>
    /// 下载
    /// </summary>
    /// <param name="fileRecordQueryCriteria"></param>
    /// <returns></returns>
    public async Task<List<ExportBase>> DownloadAsync(FileRecordQueryCriteria fileRecordQueryCriteria)
    {
        var conditionalModels = fileRecordQueryCriteria.ApplyQueryConditionalModel();
        var fileRecords = await TableWhere(conditionalModels).ToListAsync();
        List<ExportBase> fileRecordExports = new List<ExportBase>();
        fileRecordExports.AddRange(fileRecords.Select(x => new FileRecordExport
        {
            Id = x.Id,
            Description = x.Description,
            ContentType = x.ContentType,
            ContentTypeName = x.ContentTypeName,
            ContentTypeNameEn = x.ContentTypeNameEn,
            OriginalName = x.OriginalName,
            NewName = x.NewName,
            FilePath = x.FilePath,
            Size = x.Size,
            CreateTime = x.CreateTime
        }));
        return fileRecordExports;
    }

    #endregion
}
