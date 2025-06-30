using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Message.Email;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Message;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Message.Email;

namespace BearPlatform.IBusiness.Message.Email;

/// <summary>
///  邮件消息模板
/// </summary>
public interface IEmailMessageTemplateService : IBaseServices<EmailMessageTemplate>
{

    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<EmailMessageTemplateDTO>> GetPageAsync(EmailMessageTemplateParam param);
    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmailMessageTemplateInfo> GetInfoAsync(long id);


    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateEmailMessageTemplateParam param);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> UpdateAsync(UpdateEmailMessageTemplateParam param);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(HashSet<long> ids);

    #endregion

    #region 扩展接口

    #endregion

}
