using System.Collections.Generic;
using System.Threading.Tasks;
using BearPlatform.Common.Model;
using BearPlatform.Entity.Core.Permission.User;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Permission.User;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Permission;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Permission.User;
using Microsoft.AspNetCore.Http;

namespace BearPlatform.IBusiness;

/// <summary>
/// 用户接口
/// </summary>
public interface IUserService : IBaseServices<User>
{
    #region 基础接口

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<PagedResults<UserDTO>> GetPageAsync(UserParam param);

    /// <summary>
    /// 获取详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<UserInfo> GetInfoAsync(long id);
    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> AddAsync(UpdateUserParam param);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    Task<long> UpdateAsync(UpdateUserParam param);

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<int> DeleteAsync(HashSet<long> ids);

    #endregion

    #region 扩展接口
    /// <summary>
    /// 查找用户
    /// </summary>
    /// <param name="userName">用户名</param>
    /// <returns>用户实体</returns>
    Task<UserVo> QueryByNameAsync(string userName);


    /// <summary>
    /// 根据部门ID查找用户
    /// </summary>
    /// <param name="deptIds"></param>
    /// <returns></returns>
    Task<List<UserVo>> QueryByDeptIdsAsync(List<long> deptIds);

    /// <summary>
    /// 修改个人中心信息
    /// </summary>
    /// <param name="updateUserCenterDto"></param>
    /// <returns></returns>
    Task<long> UpdateCenterAsync(UpdateUserCenterDto updateUserCenterDto);

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="userPassDto"></param>
    /// <returns></returns>
    Task<long> UpdatePasswordAsync(UpdateUserPassDto userPassDto);


    /// <summary>
    /// 修改邮箱
    /// </summary>
    /// <param name="updateUserEmailDto"></param>
    /// <returns></returns>
    Task<long> UpdateEmailAsync(UpdateUserEmailDto updateUserEmailDto);

    /// <summary>
    /// 修改头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    Task<long> UpdateAvatarAsync(IFormFile file);

    #endregion
}
