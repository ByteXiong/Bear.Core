using System;
using BearPlatform.Common.Enums;
using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity
{
    /// <summary>
    /// 表格重写
    /// </summary>
    [SugarTable("table_form_item")]
    //[Tenant(AppConfig.TenantTable)]
    public class TableFormItem : BaseEntity<long>
    {

        /// <summary>
        ///  视图Id
        /// </summary>
        public long FormId { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Label { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Prop { get; set; }

        /// <summary>
        /// 字段验证
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Rules { get; set; }




        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Int32 Sort { get; set; }


        /// <summary>
        /// 组件类型
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public ColumnTypeEnum? ComponentType { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 是否自定义
        /// </summary>
        public bool IsCustom { get; set; }
        /// <summary>
        /// 多余参数 
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Attrs { get; set; }



        #region 导航

        /// <summary>
        /// 字段
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.ManyToOne, nameof(FormId), nameof(TableForm.Id))]
        public TableForm TableForm { get; set; }

        #endregion
    }
}
