namespace BearPlatform.Models.Base
{
    /// <summary>
    /// 排序
    /// </summary>
    public class SortParam
    {

        public long Id { get; set; }
        public int Sort { get; set; }
    }


    /// <summary>
    /// 排序
    /// </summary>
    public class SelectDTO
    {

        /// <summary>
        /// 主键
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string Label { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }


}
