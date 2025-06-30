using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.Entity.Core.Permission.Role;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Message.Email;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Message;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Message.Email;

namespace BearPlatform.IBusiness.Message.Email;
/// <summary>
///  邮箱账户管理
/// </summary>
public interface IEmailAccountService : IBaseServices<EmailAccount>
{

    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<EmailAccountDTO>> GetPageAsync(EmailAccountParam param);

    /// <summary>
    /// 详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<EmailAccountInfo> GetInfoAsync(long id);
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateEmailAccountParam param);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> UpdateAsync(UpdateEmailAccountParam param);

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
