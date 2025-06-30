using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Policy;
using System.Threading.Tasks;
using Asp.Versioning;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.Common;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Helper;
using BearPlatform.Common.IdGenerator;
using BearPlatform.Entity;
using BearPlatform.IBusiness;
using BearPlatform.Models;
using BearPlatform.SqlSugar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BearPlatform.Api.Controllers
{


    /// <summary>
    /// Api管理
    /// </summary>
    /// <param name="service"></param>
    [Route("/api/[controller]/[action]")]
        public class ApisController(IApisService service ) : BaseApiController
        {
        private readonly IApisService _service = service ?? throw new ArgumentNullException(nameof(service));

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<PagedResults<ApisDTO>> GetPageAsync([FromQuery] ApisParam param) => await _service.GetPageAsync(param);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<Guid> AddAsync(UpdateApisParam param) => await _service.AddAsync(param);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<Guid> UpdateAsync(UpdateApisParam param) => await _service.UpdateAsync(param);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> Delete([FromBody] HashSet<Guid> ids) => await _service.DeleteAsync(ids);



        /// <summary>
        /// 刷新Api列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [AllowAnonymous]
        [NotAudit]
        public async Task Refresh(VersionEnum version)=> await _service.Refresh(version);



        /// <summary>
        /// 获取树图
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [AllowAnonymous]
        [NotAudit]
        public async Task<List<ApisTreeSelectDTO>> TreeSelectAsync(VersionEnum version) => await _service.TreeSelectAsync(version);
    }
}
