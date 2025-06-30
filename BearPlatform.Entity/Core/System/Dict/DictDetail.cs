using BearPlatform.Entity.Base;
using SqlSugar;

namespace BearPlatform.Entity.Core.System.Dict
{
    /// <summary>
    /// 字典详情
    /// </summary>
    [SugarTable("sys_dict_detail")]
    public class DictDetail : BaseEntityNoDataScope<long>
    {
        /// <summary>
        /// 字典ID
        /// </summary>
        [SugarColumn(IsNullable = false, IsOnlyIgnoreUpdate = true)]
        public long DictId { get; set; }

        /// <summary>
        /// 字典标签
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string Label { get; set; }

        /// <summary>
        /// 字典值
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string Value { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int DictSort { get; set; }
    }
}
