using System;
using SqlSugar;

namespace BearPlatform.Entity.Base
{
    /// <summary>
    /// Serilog日志基类
    /// </summary>
    public class SerilogBase : RootKey<long>
    {
        /// <summary>
        /// 创建者名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SplitField]
        [SugarColumn(IsNullable = true)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Level { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "varcharmax,longtext,text,clob")]
        public string Message { get; set; }

        /// <summary>
        /// 消息模板
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "varcharmax,longtext,text,clob")]
        public string MessageTemplate { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        [SugarColumn(IsNullable = true, ColumnDataType = "varcharmax,longtext,text,clob")]
        public string Properties { get; set; }
    }
}
