using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Entity.Logs;
using BearPlatform.IBusiness.Monitor;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.System;
using BearPlatform.ViewModel.Core.Monitor;

namespace BearPlatform.Business.Monitor;

/// <summary>
/// 审计日志服务
/// </summary>
public class AuditInfoService : BaseServices<AuditLog>, IAuditLogService
{
    #region 基础方法

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="auditLog"></param>
    /// <returns></returns>
    public async Task<OperateResult> CreateAsync(AuditLog auditLog)
    {
        var result = await SugarRepository.SugarClient.Insertable(auditLog).SplitTable().ExecuteCommandAsync() > 0;
        return OperateResult.Result(result);
    }

    /// <summary>
    /// 批量创建
    /// </summary>
    /// <param name="auditLogs"></param>
    /// <returns></returns>
    public async Task<OperateResult> CreateListAsync(List<AuditLog> auditLogs)
    {
        var result = await SugarRepository.SugarClient.Insertable(auditLogs).SplitTable().ExecuteCommandAsync() > 0;
        return OperateResult.Result(result);
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="logQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public async Task<List<AuditLogVo>> QueryAsync(LogQueryCriteria logQueryCriteria,
        Pagination pagination)
    {
        var queryOptions = new QueryOptions<AuditLog>
        {
            Pagination = pagination,
            ConditionalModels = logQueryCriteria.ApplyQueryConditionalModel(),
            IsSplitTable = true
        };

        var auditInfos = await TablePageAsync(queryOptions);
        return App.Mapper.MapTo<List<AuditLogVo>>(auditInfos);
    }

    /// <summary>
    /// 查询当前用户
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    public async Task<List<AuditLogVo>> QueryByCurrentAsync(Pagination pagination)
    {
        Expression<Func<AuditLog, bool>> whereLambda = x => x.CreateBy == App.HttpUser.Account;


        Expression<Func<AuditLog, AuditLog>> selectExpression = x => new AuditLog
        {
            Id = x.Id,
            Description = x.Description,
            RequestIp = x.RequestIp,
            IpAddress = x.IpAddress,
            OperatingSystem = x.OperatingSystem,
            DeviceType = x.DeviceType,
            BrowserName = x.BrowserName,
            Version = x.Version,
            ExecutionDuration = x.ExecutionDuration,
            CreateTime = x.CreateTime
        };
        var queryOptions = new QueryOptions<AuditLog>
        {
            Pagination = pagination,
            WhereLambda = whereLambda,
            SelectExpression = selectExpression,
            IsSplitTable = true
        };
        var auditInfos = await TablePageAsync(queryOptions);
        return App.Mapper.MapTo<List<AuditLogVo>>(auditInfos);
    }

    #endregion
}
