using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Attributes;
using BearPlatform.Entity.Core.System;
using BearPlatform.Models.Base;

namespace BearPlatform.Models.Dto.Core.System;

/// <summary>
/// 应用密钥Dto
/// </summary>
[AutoMapping(typeof(AppSecret), typeof(CreateUpdateAppSecretDto))]
public class CreateUpdateAppSecretDto : BaseEntityDTO<long>
{
    /// <summary>
    /// 应用ID
    /// </summary>
    public string AppId { get; set; }

    /// <summary>
    /// 签名Key
    /// </summary>
    public string AppSecretKey { get; set; }

    /// <summary>
    /// 应用名称
    /// </summary>
    [Display(Name = "App.AppName")]
    [Required(ErrorMessage = "{0}required")]
    public string AppName { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string Remark { get; set; }
}
