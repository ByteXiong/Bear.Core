using Asp.Versioning;
using Autofac.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Models;
using BearPlatform.SqlSugar;
using Microsoft.AspNetCore.Mvc;
using BearPlatform.Repository.UnitOfWork;
using SqlSugar;
using System;

namespace BearPlatform.Api.Controllers.Table
{
     
    /// <summary>
    /// 数据模型
    /// </summary>
    /// <param name="unitOfWork"></param>
    [Route("/api/[controller]/{configId}/{tableof}")]
    public class TableDynamicController(IUnitOfWork unitOfWork) : BaseApiController
    {
        readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
   
        public async Task<PagedResults<dynamic>> GetPageAsync([FromQuery] PageParam param, string configId, string tableof)
        {
            var sql = $"select * from {tableof}".ToSqlFilter();
            var page = await _unitOfWork.GetDbClient().GetConnection(configId).SqlQueryable<dynamic>(sql).SearchWhere(param).ToPagedResultsAsync(param);
            return page;
        }


        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <param name="configId"></param>
        /// <param name="tableof"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<dynamic> GetInfoAsync(int id, string configId, string tableof) {

            //var columns = GetIQueryable(x => x.TableView.Tableof == tableof && x.TableView.Type == ViewTypeEnum.编辑).ToList();

            var sql = $"select * from {tableof} ".ToSqlFilter();
            var info = await _unitOfWork.GetDbClient().GetConnection(configId).SqlQueryable<dynamic>(sql)
                .Where("id = @id ", new
                {
                    id = id
                }).FirstAsync();
            return info;
        }




        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> AddAsync(Dictionary<string, object> param, string configId, string tableof)
        {

            return _unitOfWork.GetDbClient().GetConnection(configId).Insertable(param).AS(tableof).ExecuteCommand();
        }
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <param name="configId"></param>
        /// <param name="tableof"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> UpdateAsync(Dictionary<string, object> param, string configId, string tableof)
        {
            return _unitOfWork.GetDbClient().GetConnection(configId).Updateable(param).AS(tableof).WhereColumns("id").ExecuteCommand();
        }
        /// <summary>
        /// 删除表头
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="configId"></param>
        /// <param name="tableof"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> DeleteAsync(decimal[] ids, string configId, string tableof)
        {
            return _unitOfWork.GetDbClient().GetConnection(configId).Deleteable<object>().AS(tableof).Where("id in (@id) ", new { id = ids }).ExecuteCommand();//批量
        }
    }
}
