using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.System;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.System;

/// <summary>
/// 文件记录Dto
/// </summary>
[AutoMapping(typeof(FileRecord), typeof(CreateUpdateFileRecordDto))]
public class CreateUpdateFileRecordDto : BaseEntityDTO<long>
{
    /// <summary>
    /// 描述 Sys.Description
    /// </summary>
    [Display(Name = "Sys.Description")]
    [Required(ErrorMessage = "{0}required")]
    public string Description { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// 文件类型名称
    /// </summary>
    public string ContentTypeName { get; set; }

    /// <summary>
    /// 文件类型名称(EN)
    /// </summary>
    public string ContentTypeNameEn { get; set; }

    /// <summary>
    /// 源名称
    /// </summary>
    public string OriginalName { get; set; }

    /// <summary>
    /// 新名称
    /// </summary>
    public string NewName { get; set; }

    /// <summary>
    /// 存储路径
    /// </summary>
    public string FilePath { get; set; }

    /// <summary>
    /// 文件大小
    /// </summary>
    public string Size { get; set; }
}
