using System.Collections.Generic;
using BearPlatform.SqlSugar;
namespace BearPlatform.Models
{

    public class TableFormItemParam : PageParam
    {
        public string KeyWord { get; set; }
    }


    public class DataFormItem{

        /// <summary>
        ///  表名
        /// </summary>
        public string TableName { get; set; }


        /// <summary>
        ///  注释
        /// </summary>
        public string Common { get; set; }
        /// <summary>
        ///  字段
        /// </summary>
        public string FormItemKey { get; set; }
        /// <summary>
        ///  描述
        /// </summary>
        public string Type { get; set; }


        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNull { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 精度
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

    }

    public class TableDataFormParam 
    {
        
        public long Id { get; set; }
    }

    public class UpdateTableDataParam: TableDataFormParam
    {

        public Dictionary<string,string> Data { get; set; }


    }
    public class SetAttrsFormItemParam
    {
        /// <summary>
        /// 字段Id
        /// </summary>
        public long  Id { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string Attrs { get; set; }

    }


}
