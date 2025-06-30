using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Extensions;
using BearPlatform.Entity;
using BearPlatform.IBusiness.Table;
using BearPlatform.Models;
using BearPlatform.Models.Base;
using SqlSugar;

namespace BearPlatform.Business.Table
{
    /// <summary>
    /// 模型
    /// </summary>
    public class TableFormItemService : BaseServices<TableFormItem> , ITableFormItemService
    {

        /// <summary>
        /// 设置FormItem
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [UseTran]
        public async Task<TableFormItem> SetFormItemAsync(TableFormItem param)
        {
            if (param.Prop == null)
            {
                param.IsCustom = true;
                //param.Key = IdHelper.GetId();
            }
            //else {
            //    //回填数据库注释 /页面的字段很少 可能会吧详细注释覆盖
            //    var view = await GetIQueryable(x => x.Id == param.ViewId).Select(x => new { x.ConfigId, x.Tableof, x.Type }).FirstAsync();
            //        if (view.Type == ViewTypeEnum.主页 && !string.IsNullOrEmpty(view.ConfigId) && !string.IsNullOrEmpty(view.Tableof) && !string.IsNullOrEmpty(param.Key) && !string.IsNullOrEmpty(param.Title)) {
            //           _unitOfWork.GetDbClient().GetConnection(view.ConfigId).DbMaintenance.AddColumnRemark(param.Key, view.Tableof, param.Title);
            //        }
            //  }
            await SugarClient.Storageable(param).ExecuteReturnEntityAsync();
            SugarClient.Ado.CommitTran();
            return param;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public TableFormItemService() 
        {
        }
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task SetSortAsync(List<SortParam> param)
        {
            var entity = param.Select(x => new TableFormItem { Id = x.Id, Sort = x.Sort }).ToList();
            await UpdateAsync(entity, x => x.Sort);
        }


        /// <summary>
        /// 设置高阶字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task SetAttrsAsync(SetAttrsFormItemParam param)
        {
            //await UpdateAsync(x => x.Id == param.Id, x => new TableFormItem
            //{
            //    Attrs = param.Attrs
            //});
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long[] ids)
        {
          await  LogicDeleteAsync<I18n>(x => ids.Contains(x.Id));
    
    }

    }
}
