using System.Collections.Generic;

namespace BearPlatform.Common.Exception
{

    /// <summary>
    /// 业务异常
    /// 注:并不会当作真正的异常处理,仅为方便返回前端错误提示信息
    /// </summary>
    public class BusException :  System.Exception
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="message">错误信息</param>
        /// <param name="errorCode">错误代码</param>
        public BusException(string message, int errorCode = 400, Dictionary<string, string> errors = null)
            : base(message)
        {
            ErrorCode = errorCode;
            Errors = errors;
        }

        public Dictionary<string, string> Errors { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        public int ErrorCode { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public BusException()
        {
            ErrorCode = 400;
        }




    }
}
