using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.IBusiness.Message.Email;
using BearPlatform.IBusiness.System;
using MailKit.Net.Smtp;
using MailKit.Security;

namespace BearPlatform.Business.Message.Email;

/// <summary>
/// SMTP构造
/// </summary>
public class SmtpBuilder : ISmtpBuilder
{
    #region Fields

    private readonly IEmailAccountService _emailAccountService;
    private readonly ISettingService _settingService;

    #endregion

    #region Ctor

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="emailAccountService"></param>
    /// <param name="settingService"></param>
    public SmtpBuilder(IEmailAccountService emailAccountService, ISettingService settingService)
    {
        _emailAccountService = emailAccountService;
        _settingService = settingService;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Create a new SMTP client for a specific email account
    /// </summary>
    /// <param name="emailAccount">Email account to use. If null, then would be used EmailAccount by default</param>
    /// <returns>
    /// A task that represents the asynchronous operation
    /// The task result contains the an SMTP client that can be used to send email messages
    /// </returns>
    public virtual async Task<SmtpClient> BuildAsync(EmailAccount emailAccount = null)
    {
        if (emailAccount is null)
        {
            //var defaultEmailAccountId = AppSettings.GetValue("DefaultEmailAccountId");//系统默认发送邮箱ID
            var value = await _settingService.GetSettingValue<long>("DefaultEmailAccountId");
            var defaultEmailAccountId = value;
            emailAccount =
                await _emailAccountService.TableWhere(x => x.Id == defaultEmailAccountId).FirstAsync()
                ?? throw new Exception("Email account could not be loaded");
        }

        var client = new SmtpClient
        {
            ServerCertificateValidationCallback = ValidateServerCertificate
        };

        try
        {
            await client.ConnectAsync(
                emailAccount.Host,
                emailAccount.Port,
                emailAccount.EnableSsl
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTlsWhenAvailable);

            if (emailAccount.UseDefaultCredentials)
            {
                await client.AuthenticateAsync(CredentialCache.DefaultNetworkCredentials);
            }
            else if (!string.IsNullOrWhiteSpace(emailAccount.UserName))
            {
                await client.AuthenticateAsync(new NetworkCredential(emailAccount.UserName, emailAccount.Password));
            }

            return client;
        }
        catch (Exception ex)
        {
            client.Dispose();
            throw new Exception(ex.Message, ex);
        }
    }

    /// <summary>
    /// Validates the remote Secure Sockets Layer (SSL) certificate used for authentication.
    /// </summary>
    /// <param name="sender">An object that contains state information for this validation.</param>
    /// <param name="certificate">The certificate used to authenticate the remote party.</param>
    /// <param name="chain">The chain of certificate authorities associated with the remote certificate.</param>
    /// <param name="sslPolicyErrors">One or more errors associated with the remote certificate.</param>
    /// <returns>A System.Boolean value that determines whether the specified certificate is accepted for authentication</returns>
    public virtual bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
    {
        //By default, server certificate verification is disabled.
        return true;
    }

    #endregion
}
