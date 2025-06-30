using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.Utils;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.IBusiness.Message.Email;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Message.Email;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Message;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Message.Email;

namespace BearPlatform.Business.Message.Email;
/// <summary>
/// 邮件消息模板
/// </summary>
public class EmailMessageTemplateService : BaseServices<EmailMessageTemplate>, IEmailMessageTemplateService
{


    #region 字段

    #endregion
    #region 构造函数
    public EmailMessageTemplateService() : base()
    {
    }
    #endregion
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<EmailMessageTemplateDTO>> GetPageAsync(EmailMessageTemplateParam param)
    {

        var page = await GetIQueryable().Select(x => new EmailMessageTemplateDTO
        {
        }, true).SearchWhere(param).ToPagedResultsAsync(param);
        return page;
    }

    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<EmailMessageTemplateInfo> GetInfoAsync(long id)
    {
        var entity = await GetIQueryable(x => x.Id == id).Select<EmailMessageTemplateInfo>().FirstAsync();
        return entity;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> AddAsync(UpdateEmailMessageTemplateParam param)
    {
        if (await GetIQueryable(x => x.Name == param.Name).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Name)));
        }

        var model = App.Mapper.MapTo<EmailMessageTemplate>(param);
        await AddAsync(model);
        return model.Id;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> UpdateAsync(UpdateEmailMessageTemplateParam param)
    {

        var emailMessageTemplate = await TableWhere(x => x.Id == param.Id).FirstAsync();
        if (emailMessageTemplate.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param,
                LanguageKeyConstants.EmailMessageTemplate,
                nameof(param.Id)));
        }

        if (emailMessageTemplate.Name != param.Name &&
            await TableWhere(j => j.Name == emailMessageTemplate.Name).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Name)));
        }

        var  model = App.Mapper.MapTo<EmailMessageTemplate>(param);
        await UpdateAsync(model);
        return model.Id;
    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<int> DeleteAsync(HashSet<long> ids)
    {

        var messageTemplateList = await GetIQueryable(x => ids.Contains(x.Id)).ToListAsync();
        if (messageTemplateList.Count <= 0)
        {
            throw new BusException(ValidationError.NotExist());
        }
        return await LogicDeleteAsync<EmailMessageTemplate>(x => ids.Contains(x.Id));
    }
    #endregion
    #region 扩展接口
    #endregion
}
