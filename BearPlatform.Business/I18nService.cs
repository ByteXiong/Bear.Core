using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Core;
using BearPlatform.Entity;
using BearPlatform.IBusiness;
using BearPlatform.Models;
using BearPlatform.Repository.SugarHandler;
using BearPlatform.SqlSugar;
using SqlSugar;

namespace BearPlatform.Business
{
        /// <summary>
        /// 国际化
        /// </summary>
        public class I18nService : BaseServices<I18n>, II18nService
        {
        public I18nService( ISugarRepository<I18n> repository) : base(repository)
        {
        }
    
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<I18nDTO>> GetPageAsync(I18nParam param) {
         
            var page = await GetIQueryable().Select(x => new I18nDTO {
            }, true).SearchWhere(param).ToPagedResultsAsync(param);
            return page;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<I18nInfo> GetInfoAsync(long id)
        {
            var entity = await GetIQueryable(x => x.Id == id).Select<I18nInfo>().FirstAsync();
            return entity;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(UpdateI18nParam param)
        {
            var model = App.Mapper.MapTo<I18n>(param);
            await AddAsync(model);
            return model.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> UpdateAsync(UpdateI18nParam param)
        {

            var model = App.Mapper.MapTo<I18n>(param);
            await UpdateAsync(model);
            return param.Id;
        }

        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(HashSet<long> ids)
        {
          return  await LogicDeleteAsync<I18n>(x => ids.Contains(x.Id));
        }
        /// <summary>
        /// 下拉框
        /// </summary>
        /// <returns></returns>
        public async Task<List<I18nSelectDTO>> GetSelectAsync()
        {
            var list = await GetIQueryable().OrderByDescending(x => x.Id).Select<I18nSelectDTO>().ToListAsync();
            return list;
        }


        /// <summary>
        /// 根据国际化 
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, string>> GetByLocaleAsync(string locale)
        {
            locale = locale.Replace("-", "_");

            var sql = $"SELECT     `key` , `{locale}` as `val` FROM `i18n`";
            var entity = await SugarClient.SqlQueryable<dynamic>(sql.ToSqlFilter()).ToListAsync();

            Dictionary<string, string> dic = new Dictionary<string, string>();
            entity.ForEach(x => dic.Add(x.key, x.val));
            return dic;
        }

    }
}
