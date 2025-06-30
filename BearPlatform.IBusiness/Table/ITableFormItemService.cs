using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Entity;
using BearPlatform.Models;
using BearPlatform.Models.Base;

namespace BearPlatform.IBusiness.Table
{
    public interface ITableFormItemService : IBaseServices<TableFormItem>
    {
        /// <summary>
        /// 设置FormItem
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<TableFormItem> SetFormItemAsync(TableFormItem param);

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task SetSortAsync(List<SortParam> param);

        /// <summary>
        /// 设置高阶字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task SetAttrsAsync(SetAttrsFormItemParam param);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task DeleteAsync(long[] ids);
    }
}
