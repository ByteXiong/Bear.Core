using System;
using System.Threading.Tasks;
using BearPlatform.Common.Extensions;
using BearPlatform.IBusiness.Message.Email;
using BearPlatform.IBusiness.Queued;
using BearPlatform.Models.Queries.Queued;
using Microsoft.Extensions.Logging;

namespace BearPlatform.Business.Message.Email;

/// <summary>
/// 电子邮件任务
/// </summary>
public class EmailScheduleTask : IEmailScheduleTask
{
    #region Fields

    private readonly IEmailAccountService _emailAccountService;
    private readonly IEmailSender _emailSender;
    private readonly IQueuedEmailService _queuedEmailService;
    private readonly ILogger<EmailScheduleTask> _logger;

    #endregion

    #region Ctor

    /// <summary>
    /// 
    /// </summary>
    /// <param name="emailAccountService"></param>
    /// <param name="emailSender"></param>
    /// <param name="queuedEmailService"></param>
    /// <param name="logger"></param>
    public EmailScheduleTask(IEmailAccountService emailAccountService, IEmailSender emailSender,
        IQueuedEmailService queuedEmailService, ILogger<EmailScheduleTask> logger)
    {
        _emailAccountService = emailAccountService;
        _emailSender = emailSender;
        _queuedEmailService = queuedEmailService;
        _logger = logger;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Executes a task
    /// </summary>
    public virtual async Task ExecuteAsync(long emailId = 0)
    {
        const int maxTries = 3;
        var queuedEmailQueryCriteria = new QueuedEmailQueryCriteria
        {
            MaxTries = maxTries,
            IsSend = false
        };
        if (emailId > 0)
        {
            queuedEmailQueryCriteria.Id = emailId;
        }

        var queuedEmails = await _queuedEmailService.QueryToSendMailAsync(
            queuedEmailQueryCriteria);
        foreach (var queuedEmail in queuedEmails)
        {
            var bcc = string.IsNullOrWhiteSpace(queuedEmail.Bcc)
                ? null
                : queuedEmail.Bcc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            var cc = string.IsNullOrWhiteSpace(queuedEmail.Cc)
                ? null
                : queuedEmail.Cc.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

            try
            {
                var isTrue = await _emailSender.SendEmailAsync(
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
            }
            catch (Exception exc)
            {
                _logger.LogError($"Error sending e-mail. {exc.Message}");
            }
            finally
            {
                queuedEmail.SentTries += 1;
                queuedEmail.UpdateBy = "EmailScheduleTask";
                queuedEmail.UpdateTime = DateTime.Now.ToUnixTimeStampSecond();
                await _queuedEmailService.UpdateAsync(queuedEmail);
            }
        }
    }

    #endregion
}
