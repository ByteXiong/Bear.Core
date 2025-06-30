using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.Utils;
using BearPlatform.Entity.Core.System.QuartzNet;
using BearPlatform.IBusiness.System;
using BearPlatform.Models.Dto.Core.System;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.TaskService.service;
using BearPlatform.ViewModel.Core.System.QuartzNet;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace BearPlatform.Api.Controllers.System;

/// <summary>
/// 作业调度管理
/// </summary>
[Route("/api/[controller]/[action]")]
public class QuartzNetController : BaseApiController
{
    #region 字段

    private readonly IQuartzNetService _quartzNetService;
    private readonly IQuartzNetLogService _quartzNetLogService;
    private readonly ISchedulerCenterService _schedulerCenterService;

    #endregion

    #region 构造函数

    public QuartzNetController(IQuartzNetService quartzNetService, IQuartzNetLogService quartzNetLogService,
        ISchedulerCenterService schedulerCenterService)
    {
        _quartzNetService = quartzNetService;
        _quartzNetLogService = quartzNetLogService;
        _schedulerCenterService = schedulerCenterService;
    }

    #endregion

    #region 内部接口

    /// <summary>
    /// 新增作业
    /// </summary>
    /// <param name="createUpdateQuartzNetDto"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("create")]
    [Description("Sys.Create")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ActionResultVm))]
    public async Task<ActionResult> Create(
        [FromBody] CreateUpdateQuartzNetDto createUpdateQuartzNetDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        if (createUpdateQuartzNetDto.TriggerType == TriggerType.Cron)
        {
            if (createUpdateQuartzNetDto.Cron.IsNullOrEmpty())
            {
                return Error(ValidationError.Required(createUpdateQuartzNetDto, nameof(createUpdateQuartzNetDto.Cron)));
            }

            if (!CronExpression.IsValidExpression(createUpdateQuartzNetDto.Cron))
            {
                return Error(App.L.R("{0}Error.Format", "cron"));
            }
        }
        else if (createUpdateQuartzNetDto.TriggerType == (int)TriggerType.Simple)
        {
            if (createUpdateQuartzNetDto.IntervalSecond <= 5)
            {
                return Error(App.L.R("Error.SetIntervalSeconds"));
            }
        }

        var quartzNet = await _quartzNetService.CreateAsync(createUpdateQuartzNetDto);
        if (quartzNet.IsNotNull())
        {
            if (quartzNet.IsEnable)
            {
                //开启作业任务
                await _schedulerCenterService.AddScheduleJobAsync(quartzNet);
            }
        }

        return Ok(OperateResult.Result(quartzNet.IsNotNull()));
    }

    /// <summary>
    /// 更新作业
    /// </summary>
    /// <param name="createUpdateQuartzNetDto"></param>
    /// <returns></returns>
    [HttpPut]
    [Route("edit")]
    [Description("Sys.Edit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Update(
        [FromBody] CreateUpdateQuartzNetDto createUpdateQuartzNetDto)
    {
        if (!ModelState.IsValid)
        {
            var actionError = ModelState.GetErrors();
            return Error(actionError);
        }

        if (createUpdateQuartzNetDto.TriggerType == TriggerType.Cron)
        {
            if (createUpdateQuartzNetDto.Cron.IsNullOrEmpty())
            {
                return Error(ValidationError.Required(createUpdateQuartzNetDto, nameof(createUpdateQuartzNetDto.Cron)));
            }

            if (!CronExpression.IsValidExpression(createUpdateQuartzNetDto.Cron))
            {
                return Error(App.L.R("{0}Error.Format", "cron"));
            }
        }
        else if (createUpdateQuartzNetDto.TriggerType == (int)TriggerType.Simple)
        {
            if (createUpdateQuartzNetDto.IntervalSecond <= 5)
            {
                return Error(App.L.R("Error.SetIntervalSeconds"));
            }
        }

        var result = await _quartzNetService.UpdateAsync(createUpdateQuartzNetDto);
        if (result.IsSuccess)
        {
            var quartzNet = App.Mapper.MapTo<QuartzNet>(createUpdateQuartzNetDto);
            await _schedulerCenterService.DeleteScheduleJobAsync(quartzNet);

            if (quartzNet.IsEnable)
            {
                await _schedulerCenterService.AddScheduleJobAsync(quartzNet);
            }
        }

        return Ok(result);
    }

    /// <summary>
    /// 删除作业
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

        var quartzList = await _quartzNetService.TableWhere(x => idCollection.IdArray.Contains(x.Id)).ToListAsync();
        if (quartzList.Count > 0)
        {
            var result = await _quartzNetService.DeleteAsync(quartzList);
            if (result.IsSuccess)
            {
                foreach (var item in quartzList)
                {
                    await _schedulerCenterService.DeleteScheduleJobAsync(item);
                }
            }

            return Ok(result);
        }

        return Error(ValidationError.NotExist());
    }

    /// <summary>
    /// 获取作业调度任务列表
    /// </summary>
    /// <param name="quartzNetQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("query")]
    [Description("Sys.Query")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ActionResultVm<List<QuartzNetVo>>))]
    public async Task<ActionResult> Query(QuartzNetQueryCriteria quartzNetQueryCriteria,
        Pagination pagination)
    {
        var quartzNetList = await _quartzNetService.QueryAsync(quartzNetQueryCriteria, pagination);

        foreach (var quartzNet in quartzNetList)
        {
            quartzNet.TriggerStatus = await _schedulerCenterService.GetTriggerStatus(quartzNet);
        }

        return JsonContent(quartzNetList, pagination);
    }

    /// <summary>
    /// 导出作业调度
    /// </summary>
    /// <param name="quartzNetQueryCriteria"></param>
    /// <returns></returns>
    [HttpGet]
    [Description("Sys.Export")]
    [Route("download")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(FileContentResult))]
    public async Task<ActionResult> Download(QuartzNetQueryCriteria quartzNetQueryCriteria)
    {
        var quartzNetExports = await _quartzNetService.DownloadAsync(quartzNetQueryCriteria);
        var data = new ExcelHelper().GenerateExcel(quartzNetExports, out var mimeType, out var fileName);
        return new FileContentResult(data, mimeType)
        {
            FileDownloadName = fileName
        };
    }


    /// <summary>
    /// 执行作业
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut]
    [Description("Action.ExecuteJob")]
    [Route("execute")]
    public async Task<ActionResult> Execute(long id)
    {
        if (id.IsNullOrEmpty())
        {
            return Error("id cannot be empty");
        }

        var quartzNet = await _quartzNetService.TableWhere(x => x.Id == id).FirstAsync();
        if (quartzNet.IsNull())
        {
            return Error(ValidationError.NotExist());
        }

        //开启作业任务
        quartzNet.IsEnable = true;
        if (await _quartzNetService.UpdateAsync(quartzNet))
        {
            //检查任务在内存状态
            var isTrue = await _schedulerCenterService.IsExistScheduleJobAsync(quartzNet);
            if (!isTrue)
            {
                if (await _schedulerCenterService.AddScheduleJobAsync(quartzNet))
                {
                    return Ok(OperateResult.Success());
                }

                return Ok(OperateResult.Error(App.L.R("Error.ExecutionFailed")));
            }

            return Ok(OperateResult.Error(App.L.R("Error.AlreadyRunning")));
        }

        return Error();
    }

    /// <summary>
    /// 暂停作业
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut]
    [Description("Action.PauseJob")]
    [Route("pause")]
    public async Task<ActionResult> Pause(long id)
    {
        if (id.IsNullOrEmpty())
        {
            return Error("id cannot be empty");
        }

        var quartzNet = await _quartzNetService.TableWhere(x => x.Id == id).FirstAsync();
        if (quartzNet.IsNull())
        {
            return Error(ValidationError.NotExist());
        }

        var triggerStatus = await _schedulerCenterService.GetTriggerStatus(App.Mapper.MapTo<QuartzNetVo>(quartzNet));
        if (triggerStatus == "运行中")
        {
            //检查任务在内存状态
            var isTrue = await _schedulerCenterService.IsExistScheduleJobAsync(quartzNet);
            if (isTrue && await _schedulerCenterService.PauseJob(quartzNet))
            {
                return Ok(OperateResult.Success());
            }
        }

        return Error(App.L.R("Error.PauseFailed"));
    }

    /// <summary>
    /// 恢复作业
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut]
    [Description("Action.ResumeJob")]
    [Route("resume")]
    public async Task<ActionResult> Resume(long id)
    {
        if (id.IsNullOrEmpty())
        {
            return Error("id cannot be empty");
        }

        var quartzNet = await _quartzNetService.TableWhere(x => x.Id == id).FirstAsync();
        if (quartzNet.IsNull())
        {
            return Error(ValidationError.NotExist());
        }

        var triggerStatus = await _schedulerCenterService.GetTriggerStatus(App.Mapper.MapTo<QuartzNetVo>(quartzNet));
        if (triggerStatus == "暂停")
        {
            //检查任务在内存状态
            var isTrue = await _schedulerCenterService.IsExistScheduleJobAsync(quartzNet);
            if (isTrue && await _schedulerCenterService.ResumeJob(quartzNet))
            {
                return Ok(OperateResult.Success());
            }
        }

        return Error(App.L.R("Error.RestoreFailed"));
    }


    /// <summary>
    /// 作业调度执行日志
    /// </summary>
    /// <param name="quartzNetLogQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("logs/query")]
    [Description("Action.ExecutionLogJob")]
    public async Task<ActionResult> QueryLog(QuartzNetLogQueryCriteria quartzNetLogQueryCriteria,
        Pagination pagination)
    {
        var quartzNetLogList = await _quartzNetLogService.QueryAsync(quartzNetLogQueryCriteria, pagination);

        return JsonContent(quartzNetLogList, pagination);
    }

    /// <summary>
    /// 导出作业日志
    /// </summary>
    /// <param name="quartzNetLogQueryCriteria"></param>
    /// <returns></returns>
    [HttpGet]
    [Description("Sys.Export")]
    [Route("logs/download")]
    public async Task<ActionResult> Download(QuartzNetLogQueryCriteria quartzNetLogQueryCriteria)
    {
        var quartzNetLogExports = await _quartzNetLogService.DownloadAsync(quartzNetLogQueryCriteria);
        var data = new ExcelHelper().GenerateExcel(quartzNetLogExports, out var mimeType, out var fileName);
        return new FileContentResult(data, mimeType)
        {
            FileDownloadName = fileName
        };
    }

    #endregion
}
