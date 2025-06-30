using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Entity;
using BearPlatform.Models;
using BearPlatform.SqlSugar;

namespace BearPlatform.IBusiness
{
    public interface II18nService : IBaseServices<I18n>
        {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedResults<I18nDTO>> GetPageAsync(I18nParam param);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="createUpdateI18nDto"></param>
        /// <returns></returns>
        Task<long> AddAsync(UpdateI18nParam createUpdateI18nDto);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="createUpdateI18nDto"></param>
        /// <returns></returns>
        Task<long> UpdateAsync(UpdateI18nParam createUpdateI18nDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(HashSet<long> ids);



        /// <summary>
        /// 根据国际化 
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetByLocaleAsync(string locale);
    }
}
