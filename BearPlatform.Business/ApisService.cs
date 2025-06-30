using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using BearPlatform.Common.IdGenerator;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Entity;
using BearPlatform.IBusiness;
using BearPlatform.Models;
using BearPlatform.Repository.SugarHandler;
using BearPlatform.SqlSugar;
using Newtonsoft.Json;
using SqlSugar;

namespace BearPlatform.Business
{
    /// <summary>
    /// Api管理
    /// </summary>
    public class ApisService : BaseServices<Apis>, IApisService
    {
        public ApisService(ISugarRepository<Apis> repository) : base(repository)
        {
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<ApisDTO>> GetPageAsync(ApisParam param)
        {

            var page = await GetIQueryable().WhereIF(param.Version!=null, x => x.Version==(int)param.Version)
                .Select(x => new ApisDTO
            {
            }, true).SearchWhere(param).ToPagedResultsAsync(param);
            return page;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApisInfo> GetInfoAsync(Guid id)
        {
            var entity = await GetIQueryable(x => x.Id == id).Select<ApisInfo>().FirstAsync();
            return entity;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> AddAsync(UpdateApisParam param)
        {
            var model = App.Mapper.MapTo<Apis>(param);
            await AddAsync(model);
            return model.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateAsync(UpdateApisParam param)
        {

            var model = App.Mapper.MapTo<Apis>(param);
            await UpdateAsync(model);
            return param.Id;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(HashSet<Guid> ids)
        {
            return await LogicDeleteAsync<Apis>(x => ids.Contains(x.Id));
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        [UseTran]
        public async Task Refresh(VersionEnum version)
        {
            var url = $"http://localhost:3000/swagger/v{(int)version}/swagger.json";
            var swaggerJson = HttpHelper.GetData(url);
            var doc = JsonConvert.DeserializeObject<SwaggerDocument>(swaggerJson);
            var ver = Convert.ToInt32( doc.Info.Version.Split('.')[0]);
            List<Apis> apis = new List<Apis>();
            await SugarClient.Deleteable<Apis>(x => x.Version == ver).ExecuteCommandAsync();
            doc.Paths.ForEach(api =>
            {
                apis.Add(new Apis()
                {
                    Id = StringToUuidConverter.GenerateVersion5Uuid(api.Key+ doc.Info.Version),
                    Group = api.Value.Values?.FirstOrDefault()?.Tags?.FirstOrDefault(),
                    Url = api.Key,
                    Description = api.Value.Values?.FirstOrDefault()?.Summary ?? "请添加描述",
                    Method = api.Value.Keys?.FirstOrDefault() ?? "default",
                    Version = ver,
                });
            });
            await AddAsync(apis);
        }
        /// <summary>
        /// 获取树图
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public async Task<List<ApisTreeSelectDTO>> TreeSelectAsync(VersionEnum version) {
            var list =await GetIQueryable(x=> x.Version== (int)version).ToListAsync();
            var tree=   list.GroupBy(x => x.Group).Select(x => new ApisTreeSelectDTO() { 
                Id=Guid.NewGuid(),
                Label = x.Key, 
                Children = x.Select(y => new ApisTreeSelectDTO() { Label = y.Description, Id = y.Id }).ToList() }).ToList();
            return tree;
        }
    }
}
