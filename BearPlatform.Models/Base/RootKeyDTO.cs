namespace BearPlatform.Models.Base
{
    /// <summary>
    /// 泛型主键
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RootKeyDTO<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 主键
        /// </summary>
        public T Id { get; set; }
    }
}
