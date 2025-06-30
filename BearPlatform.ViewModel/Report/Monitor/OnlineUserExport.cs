using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Model;

namespace BearPlatform.ViewModel.Report.Monitor;

/// <summary>
/// 在线用户导出模板
/// </summary>
public class OnlineUserExport : ExportBase
{
    /// <summary>
    /// 用户名称
    /// </summary>
    [Display(Name = "Online.Account")]
    public string Account { get; set; }

    /// <summary>
    /// 用户昵称
    /// </summary>
    [Display(Name = "Online.NickName")]
    public string NickName { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    [Display(Name = "Online.Account")]
    public string Dept { get; set; }

    /// <summary>
    /// 登录IP
    /// </summary>
    [Display(Name = "Online.LoginIp")]
    public string Ip { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    [Display(Name = "Online.IpAddress")]
    public string Address { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    [Display(Name = "Online.OperatingSystem")]
    public string OperatingSystem { get; set; }

    /// <summary>
    /// 设备类型
    /// </summary>
    [Display(Name = "Online.DeviceType")]
    public string DeviceType { get; set; }

    /// <summary>
    /// 浏览器名称
    /// </summary>
    [Display(Name = "Online.BrowserName")]
    public string BrowserName { get; set; }

    /// <summary>
    /// 版本号
    /// </summary>
    [Display(Name = "Online.Version")]
    public string Version { get; set; }

    /// <summary>
    /// 登录时间
    /// </summary>
    [Display(Name = "Online.LoginTime")]
    public string LoginTime { get; set; }

    /// <summary>
    /// 令牌
    /// </summary>
    [Display(Name = "Online.AccessToken")]
    public string AccessToken { get; set; }
}
