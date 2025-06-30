using BearPlatform.Common.Attributes;
using BearPlatform.Entity;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;
using SqlSugar;

namespace BearPlatform.Models
{
    public class I18nParam : PageParam
    {

    }

    /// <summary>
    ///  
    /// </summary>
    [AutoMapping(typeof(I18nDTO), typeof(I18n))]
    public class I18nDTO : RootKeyDTO<long>
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        /// 中文
        /// </summary>
        [SugarColumn(ColumnName = "zh-cn", IsNullable = false)]
        public string ZhCn { get; set; }
        /// <summary>
        /// 英文
        /// </summary>
        [SugarColumn(ColumnName = "en-us", IsNullable = false)]
        public string EnUs { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Source { get; set; }
        /// <summary>
        /// 访问次数
        /// </summary>
        public int Count { get; set; }

    }

    [AutoMapping(typeof(I18nInfo), typeof(I18n))]
    public class I18nInfo : I18n
    {

    }

    [AutoMapping(typeof(UpdateI18nParam), typeof(I18n))]
    public class UpdateI18nParam : I18n
    {
    }
    public class I18nSelectDTO
    {
    }
}
