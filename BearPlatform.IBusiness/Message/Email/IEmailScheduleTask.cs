using System.Threading.Tasks;

namespace BearPlatform.IBusiness.Message.Email;

/// <summary>
/// 邮件任务计划
/// </summary>
public interface IEmailScheduleTask
{
    /// <summary>
    /// Executes a task
    /// </summary>
    /// <param name="emailId">邮件队列ID</param>
    /// <returns></returns>
    Task ExecuteAsync(long emailId = 0);
}
