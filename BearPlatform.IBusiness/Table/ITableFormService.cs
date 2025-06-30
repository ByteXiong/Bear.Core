using System.Threading.Tasks;
using BearPlatform.Entity;
using BearPlatform.Models;

namespace BearPlatform.IBusiness.Table
{
    public interface ITableFormService : IBaseServices<TableForm>
    {

        #region 表头信息设置
        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<TableForm> GetTableFormAsync(TableFormParam param);


        /// <summary>
        /// 添加模型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<long> AddAsync(UpdateTableFormParam param);

        /// <summary>
        /// 编辑模型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<long> UpdateAsync(UpdateTableFormParam param);

   
        #endregion

        #region 信息获取
        /// <summary>
        /// 表头信息获取
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<TableForm> GetFormAsync(TableFormParam param);
        #endregion
    }
}
