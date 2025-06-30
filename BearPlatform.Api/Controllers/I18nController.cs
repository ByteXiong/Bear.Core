using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Asp.Versioning;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common.Attributes;
using BearPlatform.IBusiness;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Permission;
using BearPlatform.SqlSugar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BearPlatform.Api.Controllers
{


    /// <summary>
    /// 表字段
    /// </summary>
    /// <param name="service"></param>
    [Route("/api/[controller]/[action]")]
        public class I18nController(II18nService service) : BaseApiController
        {


        private readonly II18nService _service = service ?? throw new ArgumentNullException(nameof(service));

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<PagedResults<I18nDTO>> GetPageAsync([FromQuery] I18nParam param) => await _service.GetPageAsync(param);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> AddAsync(UpdateI18nParam param) => await _service.AddAsync(param);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> UpdateAsync(UpdateI18nParam param) => await _service.UpdateAsync(param);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> Delete([FromBody] HashSet<long> ids) => await _service.DeleteAsync(ids);


        /// <summary>
        /// 根据国际化 
        /// </summary>
        /// <param name="locale"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [AllowAnonymous]
        [NotAudit]
        public async Task<Dictionary<string, string>> GetByLocaleAsync(string locale) => await _service.GetByLocaleAsync(locale);

    }
}
