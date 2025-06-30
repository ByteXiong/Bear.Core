using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Enums;
using BearPlatform.Entity;
using BearPlatform.Models;
using BearPlatform.SqlSugar;

namespace BearPlatform.IBusiness
{
    public interface IApisService : IBaseServices<Apis>
    {
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        Task<PagedResults<ApisDTO>> GetPageAsync(ApisParam param);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="createUpdateApisDto"></param>
        /// <returns></returns>
        Task<Guid> AddAsync(UpdateApisParam createUpdateApisDto);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="createUpdateApisDto"></param>
        /// <returns></returns>
        Task<Guid> UpdateAsync(UpdateApisParam createUpdateApisDto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<int> DeleteAsync(HashSet<Guid> ids);


        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        Task Refresh(VersionEnum version);

        /// <summary>
        /// 获取树图
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        Task<List<ApisTreeSelectDTO>> TreeSelectAsync(VersionEnum version);


    }
}
