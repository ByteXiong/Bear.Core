using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Extensions;
using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity
{
    /// <summary>
    /// 表格重写
    /// </summary>
    [SugarTable("table_view")]
    //[Tenant(AppConfig.TenantTable)]   
    public class TableView : BaseEntity<long>
    {


        /// <summary>
        /// 配置库
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ConfigId { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Router { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        [Required]
        public string Tableof { get; set; }

     


        /// <summary>
        /// 多余参数 
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Attrs { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string ColumnsString { get; set; }

        /// <summary>
        /// 默认排序字段
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string SortString { get; set; }

        #region 扩展字段
        /// <summary>
        /// 字段
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<TableColumn> Columns
        {
            get { return ColumnsString?.ToObject<List<TableColumn>>(); }
            set { ColumnsString = value.ToJson(); }
        }
        /// <summary>
        /// 多列排序
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public IDictionary<string, OrderTypeEnum> Sorts {
            get { return SortString?.ToObject<IDictionary<string, OrderTypeEnum>>(); }
            set { SortString = value.ToJson(); }
        }


        #endregion

    }

    /// <summary>
    /// 表字段重写
    /// </summary>
    public class TableColumn
    {
      
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        public string Prop { get; set; }
        /// <summary>
        /// 搜索类型
        /// </summary>
        public ConditionalType? SearchType { get; set; }
   

        /// <summary>
        /// 字段描述
        /// </summary>
        public string ColumnTypeDetail { get; set; }
        /// <summary>
        /// 字段验证
        /// </summary>
        public string ColumnTypeRules { get; set; }

        /// <summary>
        /// 是否自定义
        /// </summary>
        public bool IsCustom { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public Int32 Sort { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 是否Excel
        /// </summary>
        public bool IsExcel { get; set; }
        /// <summary>
        /// 是否逻辑删除
        /// </summary>
        public bool IsEditDel { get; set; }
        /// <summary>
        /// 多余参数 
        /// </summary>
        public string Attrs { get; set; }

        /// <summary>
        /// 头部多余参数 
        /// </summary>
        public string HeadAttrsString { get; set; }
    }

  
}
