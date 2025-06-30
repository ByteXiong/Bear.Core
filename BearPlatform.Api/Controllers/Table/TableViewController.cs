using System;
using System.Threading.Tasks;
using Asp.Versioning;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common;
using BearPlatform.Entity;
using BearPlatform.IBusiness.Table;
using BearPlatform.Models;
using BearPlatform.Repository.SugarHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers.Table
{
    /// <remarks>
    ///  数据表列表
    /// </remarks>
    /// <param name="logic"></param>
    /// <exception cref="ArgumentNullException"></exception>
    [Route("/api/[controller]/[action]")]
    [AllowAnonymous]

    public class TableViewController(ITableViewService service) : BaseApiController
    {
        private readonly ITableViewService _service = service ?? throw new ArgumentNullException(nameof(service));

        #region 表头信息
        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableViewInfo> GetEditAsync([FromQuery] TableViewEditParam param) => await _service.GetEditAsync(param);

        /// <summary>
        /// 编辑模型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> SetEditAsync(UpdateTableViewParam param)=>await _service.SetEditAsync(param);
        #endregion

        #region 表头信息获取
        /// <summary>
        /// 表头信息获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableView> GetViewAsync([FromQuery] TableViewEditParam param) => await _service.GetViewAsync(param);


        #endregion
    }
}
