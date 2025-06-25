using System.Collections.Generic;
using System.Threading.Tasks;
using Bear.Core.Common.Model;
using Bear.Core.Entity.Logs;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.System;
using Bear.Core.ViewModel.Core.Monitor;

namespace Bear.Core.IBusiness.Monitor;

/// <summary>
/// 审计日志接口
/// </summary>
public interface IAuditLogService : IBaseServices<AuditLog>
{
    #region 基础接口

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="auditLog"></param>
    /// <returns></returns>
    Task<OperateResult> CreateAsync(AuditLog auditLog);


    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="auditLogs"></param>
    /// <returns></returns>
    Task<OperateResult> CreateListAsync(List<AuditLog> auditLogs);


    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="logQueryCriteria"></param>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<List<AuditLogVo>> QueryAsync(LogQueryCriteria logQueryCriteria, Pagination pagination);

    /// <summary>
    /// 查询(个人)
    /// </summary>
    /// <param name="pagination"></param>
    /// <returns></returns>
    Task<List<AuditLogVo>> QueryByCurrentAsync(Pagination pagination);

    #endregion
}
