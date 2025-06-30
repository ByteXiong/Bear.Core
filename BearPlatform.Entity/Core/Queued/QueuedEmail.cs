using System;
using BearPlatform.Common.Enums;
using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity.Core.Queued
{
    /// <summary>
    /// 邮件队列
    /// </summary>
    [SugarTable("queued_email")]
    public class QueuedEmail : BaseEntity<long>
    {
        /// <summary>
        /// 发件邮箱
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string From { get; set; }

        /// <summary>
        /// 发件人名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string FromName { get; set; }

        /// <summary>
        /// 收件邮箱
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string To { get; set; }

        /// <summary>
        /// 收件人名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ToName { get; set; }

        /// <summary>
        /// 回复邮箱
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ReplyTo { get; set; }

        /// <summary>
        /// 回复人名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ReplyToName { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public QueuedEmailPriority Priority { get; set; }

        /// <summary>
        /// 抄送
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Cc { get; set; }

        /// <summary>
        /// 密件抄送
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Bcc { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Subject { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [SugarColumn(Length = 4000, IsNullable = false)]
        public string Body { get; set; }

        /// <summary>
        /// 发送上限次数
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int SentTries { get; set; }

        /// <summary>
        /// 是否已发送
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool? IsSend { get; set; }

        /// <summary>
        /// 发送时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public DateTime? SendTime { get; set; }

        /// <summary>
        /// 发件邮箱ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public long EmailAccountId { get; set; }
    }
}
