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
using Bear.Core.Api.Controllers.Base;
using Bear.Core.Common;
using Bear.Core.Common.Attributes;
using Bear.Core.Common.Enums;
using Bear.Core.Common.Exception;
using Bear.Core.Common.Extensions;
using Bear.Core.Common.Global;
using Bear.Core.Common.Helper;
using Bear.Core.Common.IdGenerator;
using Bear.Core.Entity;
using Bear.Core.IBusiness;
using Bear.Core.Models;
using Bear.Core.SqlSugar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Bear.Core.Api.Controllers
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
