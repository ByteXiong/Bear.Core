using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Model;

namespace BearPlatform.ViewModel.Report.System;

/// <summary>
/// 任务调度日志导出模板
/// </summary>
public class QuartzNetLogExport : ExportBase
{
    /// <summary>
    /// 任务ID
    /// </summary>
    [Display(Name = "TaskLog.TaskId")]
    public long TaskId { get; set; }

    /// <summary>
    /// 任务名称
    /// </summary>
    [Display(Name = "Task.TaskName")]
    public string TaskName { get; set; }

    /// <summary>
    /// 任务组
    /// </summary>
    [Display(Name = "Task.TaskGroup")]
    public string TaskGroup { get; set; }

    /// <summary>
    /// 程序集名称
    /// </summary>
    [Display(Name = "Task.AssemblyName")]
    public string AssemblyName { get; set; }

    /// <summary>
    /// 执行类
    /// </summary>
    [Display(Name = "Task.ClassName")]
    public string ClassName { get; set; }

    /// <summary>
    /// Cron表达式
    /// </summary>
    [Display(Name = "Task.Cron")]
    public string Cron { get; set; }

    /// <summary>
    /// 异常详情
    /// </summary>
    [Display(Name = "TaskLog.ExceptionDetail")]
    public string ExceptionDetail { get; set; }

    /// <summary>
    /// 执行耗时
    /// </summary>
    [Display(Name = "TaskLog.ExecutionDuration")]
    public long ExecutionDuration { get; set; }

    /// <summary>
    /// 执行传参
    /// </summary>
    [Display(Name = "Task.RunParams")]
    public string RunParams { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    [Display(Name = "TaskLog.IsSuccess")]
    public bool IsSuccess { get; set; }
}
