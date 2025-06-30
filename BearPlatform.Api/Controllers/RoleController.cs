using Asp.Versioning;
using BearPlatform.Api.Controllers.Base;
using BearPlatform.IBusiness.Permission;
using BearPlatform.SqlSugar;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BearPlatform.Common.Attributes;
using Microsoft.AspNetCore.Authorization;
using BearPlatform.Models.Permission;

namespace BearPlatform.Api.Controllers
{

    /// <summary>
    /// 角色
    /// </summary>
    [Route("/api/[controller]/[action]")]
    public class RoleController(IRoleService service) : BaseApiController
    {
        #region 字段

        private readonly IRoleService _service = service ?? throw new ArgumentNullException(nameof(service));
        #endregion

        #region 内部接口

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [AllowAnonymous]
        [NotAudit]
        public async Task<PagedResults<RoleDTO>> GetPageAsync([FromQuery] RoleParam param) => await _service.GetPageAsync(param);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> AddAsync(UpdateRoleParam param) => await _service.AddAsync(param);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> UpdateAsync(UpdateRoleParam param) => await _service.UpdateAsync(param);
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> Delete([FromBody] HashSet<long> ids) => await _service.DeleteAsync(ids);
        #endregion

        #region 接口扩展
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [AllowAnonymous]
        [NotAudit]
        public async Task<RolePermissionParam> GetPermissionAsync(long roleId) => await _service.GetPermissionAsync(roleId);
        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        [AllowAnonymous]
        [NotAudit]
        public async Task SetPermissionAsync(RolePermissionParam param) => await _service.SetPermissionAsync(param);
        #endregion
    }

}
