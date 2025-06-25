using System.Collections.Generic;
using System.Threading.Tasks;
using Bear.Core.Common.Model;
using Bear.Core.Entity.Core.Message.Email;
using Bear.Core.Models;
using Bear.Core.Models.Dto.Core.Message.Email;
using Bear.Core.Models.Queries.Common;
using Bear.Core.Models.Queries.Message;
using Bear.Core.SqlSugar;
using Bear.Core.ViewModel.Core.Message.Email;

namespace Bear.Core.IBusiness.Message.Email;

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
