using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Helper;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.Utils;
using BearPlatform.Entity.Core.Queued;
using BearPlatform.IBusiness.Message.Email;
using BearPlatform.IBusiness.Queued;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Queued;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Queued;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Queued;
using Microsoft.Extensions.Logging;

namespace BearPlatform.Business.Queued;

/// <summary>
/// 邮件队列
/// </summary>
public class QueuedEmailService : BaseServices<QueuedEmail>, IQueuedEmailService
{


    #region 字段

    private readonly IEmailMessageTemplateService _emailMessageTemplateService;
    private readonly IEmailAccountService _emailAccountService;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<QueuedEmailService> _logger;

    #endregion
    #region 构造函数
    public QueuedEmailService(IEmailMessageTemplateService emailMessageTemplateService,
            IEmailAccountService emailAccountService, IEmailSender emailSender, ILogger<QueuedEmailService> logger)
    {
        _emailMessageTemplateService = emailMessageTemplateService;
        _emailAccountService = emailAccountService;
        _emailSender = emailSender;
        _logger = logger;
    }
    #endregion
    #region 基础方法
    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<QueuedEmailDTO>> GetPageAsync(QueuedEmailParam param)
    {

        var page = await GetIQueryable().Select(x => new QueuedEmailDTO
        {
        }, true).SearchWhere(param).ToPagedResultsAsync(param);
        return page;
    }

    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<QueuedEmailInfo> GetInfoAsync(long id)
    {
        var entity = await GetIQueryable(x => x.Id == id).Select<QueuedEmailInfo>().FirstAsync();
        return entity;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> AddAsync(UpdateQueuedEmailParam param)
    {
        var emailAccount = await _emailAccountService.TableWhere(x => x.Id == param.EmailAccountId)
              .SingleAsync();
        if (emailAccount.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param,
                LanguageKeyConstants.EmailAccount,
                nameof(param.Id)));
        }

        param.From = emailAccount.Email;
        param.FromName = emailAccount.DisplayName;
        var model = App.Mapper.MapTo<QueuedEmail>(param);
        var result = await AddAsync(model);
        return model.Id;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> UpdateAsync(UpdateQueuedEmailParam param)
    {

        if (!await TableWhere(x => x.Id == param.Id).AnyAsync())
        {
            throw new BusException(ValidationError.NotExist(param,
                LanguageKeyConstants.QueuedEmail,
                nameof(param.Id)));
        }

        var emailAccount = await _emailAccountService.TableWhere(x => x.Id == param.EmailAccountId)
            .SingleAsync();
        if (emailAccount.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param,
                LanguageKeyConstants.EmailAccount,
                nameof(param.EmailAccountId)));
        }

        param.From = emailAccount.Email;
        param.FromName = emailAccount.DisplayName;
        var model = App.Mapper.MapTo<QueuedEmail>(param);
        var result = await UpdateAsync(model);
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
        var emailAccounts = await GetIQueryable(x => ids.Contains(x.Id)).ToListAsync();
        if (emailAccounts.Count < 1)
        {
            throw new BusException(ValidationError.NotExist());
        }

        return await LogicDeleteAsync<QueuedEmail>(x => ids.Contains(x.Id));
    }
    #endregion

    #region 扩展方法
    /// <summary>
    /// 变更邮箱验证码
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <param name="messageTemplateName"></param>
    /// <returns></returns>
    public async Task<OperateResult> ResetEmail(string emailAddress, string messageTemplateName)
    {
        var emailMessageTemplate =
            await _emailMessageTemplateService.TableWhere(x => x.Name == messageTemplateName).FirstAsync();
        if (emailMessageTemplate.IsNull())
        {
            throw new BusException(ValidationError.NotExist());
        }

        var emailAccount = await _emailAccountService.TableWhere(x => x.Id == emailMessageTemplate.EmailAccountId)
            .SingleAsync();
        if (emailAccount.IsNull())
        {
            throw new BusException(ValidationError.NotExist());
        }

        //生成6位随机码
        var captcha = SixLaborsImageHelper.BuilEmailCaptcha(6);

        QueuedEmail queuedEmail = new QueuedEmail();
        queuedEmail.From = emailAccount.Email;
        queuedEmail.FromName = emailAccount.DisplayName;
        queuedEmail.To = emailAddress;
        queuedEmail.Priority = QueuedEmailPriority.High;
        queuedEmail.Bcc = emailMessageTemplate.BccEmailAddresses;
        queuedEmail.Subject = emailMessageTemplate.Subject;
        queuedEmail.Body = emailMessageTemplate.Body.Replace("%captcha%", captcha);
        queuedEmail.SentTries = 1;
        queuedEmail.EmailAccountId = emailAccount.Id;

        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.EmailCaptcha +
                                    queuedEmail.To.ToMd5String());
        var isTrue = await App.Cache.SetAsync(
            GlobalConstants.CachePrefix.EmailCaptcha + queuedEmail.To.ToMd5String(), captcha,
            TimeSpan.FromMinutes(5), null);

        if (isTrue)
        {
            var bcc = string.IsNullOrWhiteSpace(queuedEmail.Bcc)
                ? null
                : queuedEmail.Bcc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var cc = string.IsNullOrWhiteSpace(queuedEmail.Cc)
                ? null
                : queuedEmail.Cc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                isTrue = await _emailSender.SendEmailAsync(
                    await _emailAccountService.TableWhere(x => x.Id == queuedEmail.EmailAccountId).FirstAsync(),
                    queuedEmail.Subject,
                    queuedEmail.Body,
                    queuedEmail.From,
                    queuedEmail.FromName,
                    queuedEmail.To,
                    queuedEmail.ToName,
                    queuedEmail.ReplyTo,
                    queuedEmail.ReplyToName,
                    bcc,
                    cc);
                queuedEmail.IsSend = isTrue;
                if (isTrue)
                {
                    queuedEmail.SendTime = DateTime.Now;
                }
                // 如果开启redis并且开启消息队列功能 可以使用下面方式
                // await App.Cache.GetDatabase()
                //     .ListLeftPushAsync(MqTopicNameKey.MailboxQueue, queuedEmail.Id.ToString());
            }
            catch (Exception exc)
            {
                _logger.LogError($"Error sending e-mail. {exc.Message}");
                isTrue = false;
            }
            finally
            {
                try
                {
                    await AddAsync(queuedEmail);
                }
                catch
                {
                    // ignored
                }
            }
        }

        return OperateResult.Result(isTrue);
    }

    /// <summary>
    /// 查询 发送邮件
    /// </summary>
    /// <param name="queuedEmailQueryCriteria"></param>
    /// <returns></returns>
    public Task<List<QueuedEmail>> QueryToSendMailAsync(QueuedEmailQueryCriteria queuedEmailQueryCriteria)
    {
        return TableWhere(queuedEmailQueryCriteria.ApplyQueryConditionalModel()).ToListAsync();
    }

    #endregion
}

