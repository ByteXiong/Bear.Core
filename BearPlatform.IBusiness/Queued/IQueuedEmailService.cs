using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.Queued;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Queued;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Queued;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Queued;

namespace BearPlatform.IBusiness.Queued;

/// <summary>
///  邮件队列
/// </summary>
public interface IQueuedEmailService : IBaseServices<QueuedEmail>
{

    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<QueuedEmailDTO>> GetPageAsync(QueuedEmailParam param);

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateQueuedEmailParam param);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> UpdateAsync(UpdateQueuedEmailParam param);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(HashSet<long> ids);

    #endregion


    #region 扩展接口

    /// <summary>
    /// 重置邮箱
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <param name="messageTemplateName"></param>
    /// <returns></returns>
    Task<OperateResult> ResetEmail(string emailAddress, string messageTemplateName);


    /// <summary>
    /// 查询 发送邮件
    /// </summary>
    /// <param name="queuedEmailQueryCriteria"></param>
    /// <returns></returns>
    Task<List<QueuedEmail>> QueryToSendMailAsync(QueuedEmailQueryCriteria queuedEmailQueryCriteria);

    #endregion

}
