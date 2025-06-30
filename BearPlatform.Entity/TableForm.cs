using System.Collections.Generic;
using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity
{
    /// <summary>
    /// 表格重写
    /// </summary>
    [SugarTable("table_form")]
    //[Tenant(AppConfig.TenantTable)]   

    public class TableForm : BaseEntity<long>
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Tableof { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Router { get; set; }



        /// <summary>
        /// 多余参数 
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Attrs { get; set; }

        /// <summary>
        /// 多余参数 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ConfigId { get; set; }

        #region 导航

        /// <summary>
        /// 字段
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(TableFormItem.FormId), nameof(Id))]
        public List<TableFormItem> Items { get; set; }
        #endregion

    }
}
