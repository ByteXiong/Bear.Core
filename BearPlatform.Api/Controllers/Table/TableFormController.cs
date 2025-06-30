using System;
using System.Threading.Tasks;
using Asp.Versioning;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Entity;
using BearPlatform.IBusiness.Table;
using BearPlatform.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers.Table
{
    /// <remarks>
    ///  表单列表
    /// </remarks>
    /// <param name="service"></param>
    /// <exception cref="ArgumentNullException"></exception>
    [Route("/api/[controller]/[action]")]
    [AllowAnonymous]

    public class TableFormController(ITableFormService service) : BaseApiController
    {
        private readonly ITableFormService _service = service ?? throw new ArgumentNullException(nameof(service));

        #region 表头信息
        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableForm> GetTableFormAsync([FromQuery] TableFormParam param) => await _service.GetTableFormAsync(param);


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> AddAsync(UpdateTableFormParam param)
        {

            return await _service.AddAsync(param);
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> UpdateAsync(UpdateTableFormParam param)
        {
            return await _service.UpdateAsync(param);
        }
        #endregion

        #region 表头信息获取
        /// <summary>
        /// 表头信息获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableForm> GetFormAsync([FromQuery] TableFormParam param) => await _service.GetFormAsync(param);


        #endregion
    }
}
