using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Entity;
using BearPlatform.IBusiness.Table;
using BearPlatform.Models;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers.Table
{

    /// <summary>
    /// 表单项
    /// </summary>
    /// <param name="service"></param>
    [Route("/api/[controller]/[action]")]
    [AllowAnonymous]

    public class TableFormItemController( ITableFormItemService  service) : BaseApiController
    {


        private readonly ITableFormItemService _service = service ?? throw new ArgumentNullException(nameof(service));


        /// <summary>
        /// 设置FormItem
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableFormItem> SetFormItemAsync(TableFormItem param) => await _service.SetFormItemAsync(param);


        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task SetSortAsync(List<SortParam> param) => await _service.SetSortAsync(param);


        /// <summary>
        /// 设置高阶字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task SetAttrsAsync(SetAttrsFormItemParam param) => await _service.SetAttrsAsync(param);


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task DeleteAsync([FromBody] long[] ids) => await _service.DeleteAsync(ids);

    }
}
