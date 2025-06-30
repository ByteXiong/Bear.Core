using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Helper;
using BearPlatform.Common.IdGenerator;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.ConfigOptions;
using BearPlatform.Core.Utils;
using BearPlatform.Entity.Core.Permission.User;
using BearPlatform.IBusiness;
using BearPlatform.IBusiness.Permission;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Permission.User;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Permission;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Permission.User;
using BearPlatform.ViewModel.Report.Permission;
using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace BearPlatform.Business;

/// <summary>
/// 用户服务
/// </summary>
public class UserService : BaseServices<User>, IUserService
{
    #region 字段

    private readonly IDeptService _deptService;
    private readonly IRoleService _roleService;

    #endregion

    #region 构造函数

    /// <summary>
    /// 
    /// </summary>
    /// <param name="deptService"></param>
    /// <param name="roleService"></param>
    public UserService(IDeptService deptService, IRoleService roleService)
    {
        _deptService = deptService;
        _roleService = roleService;
    }

    #endregion

    #region 基础方法

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<UserDTO>> GetPageAsync(UserParam param)
    {

        Expression<Func<User, bool>> where = x => true;
        if (!string.IsNullOrWhiteSpace(param.KeyWord))
        {
            param.KeyWord = param.KeyWord.Trim();
            where = where.And(x => x.NickName.Contains(param.KeyWord));
        }
        var page = await GetIQueryable(where).Select(x => new UserDTO
        {
            Id = x.Id,
            AvatarPath = x.AvatarPath,
            Gender = x.Gender,
            Enabled = x.Enabled,
            Email = x.Email,
            CreateTime = x.CreateTime
        }, true).ToPagedResultsAsync(param);
        return page;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<UserInfo> GetInfoAsync(long id)
    {
        var user = await GetIQueryable(x => x.Id == id).Includes(x => x.Dept)
            .Includes(x => x.Roles)
            .Includes(x => x.Jobs).FirstAsync();

        return App.Mapper.MapTo<UserInfo>(user);
    }

    /// <summary>
    /// 创建
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> AddAsync(UpdateUserParam param)
    {
        if (await GetIQueryable(x => x.UserName == param.UserName).AnyAsync())
        {
          throw new  BusException (ValidationError.IsExist(param,
                nameof(param.UserName)));
        }

        if (await TableWhere(x => x.Email == param.Email).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Email)));
        }

        if (await TableWhere(x => x.Phone == param.Phone).AnyAsync())
        {
             throw new  BusException(ValidationError.IsExist(param,
                nameof(param.Phone)));
        }

        var user = App.Mapper.MapTo<User>(param);

        //设置用户密码
        user.Password = BCryptHelper.Hash(App.GetOptions<SystemOptions>().UserDefaultPassword);
        user.DeptId = user.Dept.Id;
        //用户
        await AddAsync(user);


        await SugarClient.Deleteable<UserRole>().Where(x => x.UserId == user.Id).ExecuteCommandAsync();
        var userRoles = new List<UserRole>();
        userRoles.AddRange(user.Roles.Select(x => new UserRole { UserId = user.Id, RoleId = x.Id }));
        await SugarClient.Insertable(userRoles).ExecuteCommandAsync();


        await SugarClient.Deleteable<UserJob>().Where(x => x.UserId == user.Id).ExecuteCommandAsync();
        var userJobs = new List<UserJob>();
        userJobs.AddRange(user.Jobs.Select(x => new UserJob { UserId = user.Id, JobId = x.Id }));
        await SugarClient.Insertable(userJobs).ExecuteCommandAsync();

        return user.Id;
    }

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> UpdateAsync(UpdateUserParam param)
    {
        //取出待更新数据
        var oldUser = await TableWhere(x => x.Id == param.Id).Includes(x => x.Roles).FirstAsync();
        if (oldUser.IsNull())
        {
             throw new  BusException(ValidationError.NotExist(param, LanguageKeyConstants.User,
                nameof(param.Id)));
        }

        if (oldUser.UserName != param.UserName &&
            await TableWhere(x => x.UserName == param.UserName).AnyAsync())
        {
             throw new  BusException(ValidationError.IsExist(param,
                nameof(param.UserName)));
        }

        if (oldUser.Email != param.Email &&
            await TableWhere(x => x.Email == param.Email).AnyAsync())
        {
             throw new  BusException(ValidationError.IsExist(param,
                nameof(param.Email)));
        }

        if (oldUser.Phone != param.Phone &&
            await TableWhere(x => x.Phone == param.Phone).AnyAsync())
        {
             throw new  BusException(ValidationError.IsExist(param,
                nameof(param.Phone)));
        }

        //验证角色等级
        var levels = oldUser.Roles.Select(x => x.Level);
        await _roleService.VerificationUserRoleLevelAsync(levels.Min());
        var user = App.Mapper.MapTo<User>(param);
        user.DeptId = user.Dept.Id;
        //更新用户
        await UpdateAsync(user, null, x => new { x.Password, x.AvatarPath, x.IsAdmin, x.PasswordReSetTime });


        await SugarClient.Deleteable<UserRole>().Where(x => x.UserId == user.Id).ExecuteCommandAsync();
        var userRoles = new List<UserRole>();
        userRoles.AddRange(user.Roles.Select(x => new UserRole { UserId = user.Id, RoleId = x.Id }));
        await SugarClient.Insertable(userRoles).ExecuteCommandAsync();


        await SugarClient.Deleteable<UserJob>().Where(x => x.UserId == user.Id).ExecuteCommandAsync();
        var userJobs = new List<UserJob>();
        userJobs.AddRange(user.Jobs.Select(x => new UserJob { UserId = user.Id, JobId = x.Id }));
        await SugarClient.Insertable(userJobs).ExecuteCommandAsync();

        //清理缓存
        await ClearUserCache(user.Id);
        return user.Id;
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(HashSet<long> ids)
    {
        if (ids.Contains(App.HttpUser.Id))
        {
             throw new  BusException(App.L.R("Error.ForbidToDeleteYourself"));
        }

        //验证角色等级
        await _roleService.VerificationUserRoleLevelAsync(await _roleService.QueryUserRoleLevelAsync(ids));


        var users = await TableWhere(x => ids.Contains(x.Id)).ToListAsync();
        foreach (var user in users)
        {
            await ClearUserCache(user.Id);
        }

        var result = await LogicDeleteAsync<User>(x => ids.Contains(x.Id));
        return 1;
    }

  


    #endregion

    #region 扩展方法


    /// <summary>
    /// 查询用户
    /// </summary>
    /// <param name="userName">邮箱 or 用户名</param>
    /// <returns></returns>
    public async Task<UserVo> QueryByNameAsync(string userName)
    {
        User user;
        if (userName.IsEmail())
        {
            user = await TableWhere(s => s.Email == userName, null, null, null, true).FirstAsync();
        }
        else
        {
            user = await TableWhere(s => s.UserName == userName, null, null, null, true).FirstAsync();
        }

        return App.Mapper.MapTo<UserVo>(user);
    }

    /// <summary>
    /// 根据部门ID查找用户
    /// </summary>
    /// <param name="deptIds"></param>
    /// <returns></returns>
    public async Task<List<UserVo>> QueryByDeptIdsAsync(List<long> deptIds)
    {
        return App.Mapper.MapTo<List<UserVo>>(
            await TableWhere(u => deptIds.Contains(u.DeptId)).ToListAsync());
    }

    /// <summary>
    /// 更新用户公共信息
    /// </summary>
    /// <param name="updateUserCenterDto"></param>
    /// <returns></returns>
    /// <exception cref="BadRequestException"></exception>
    public async Task<long> UpdateCenterAsync(UpdateUserCenterDto updateUserCenterDto)
    {
        var user = await TableWhere(x => x.Id == App.HttpUser.Id).FirstAsync();
        if (user.IsNull())
        {
             throw new  BusException(ValidationError.NotExist());
        }

        var checkUser = await TableWhere(x =>
            x.Phone == updateUserCenterDto.Phone && x.Id != App.HttpUser.Id).FirstAsync();
        if (checkUser.IsNotNull())
        {
             throw new  BusException(ValidationError.IsExist(updateUserCenterDto,
                nameof(updateUserCenterDto.Phone)));
        }

        user.NickName = updateUserCenterDto.NickName;
        user.Gender = updateUserCenterDto.Gender;
        user.Phone = updateUserCenterDto.Phone;
         await UpdateAsync(user);
        return user.Id;
    }

    /// <summary>
    /// 更新用户密码
    /// </summary>
    /// <param name="userPassDto"></param>
    /// <returns></returns>
    public async Task<long> UpdatePasswordAsync(UpdateUserPassDto userPassDto)
    {
        var rsaOptions = App.GetOptions<RsaOptions>();
        var rsaHelper = new RsaHelper(rsaOptions.PrivateKey, rsaOptions.PublicKey);
        string oldPassword = rsaHelper.Decrypt(userPassDto.OldPassword);
        string newPassword = rsaHelper.Decrypt(userPassDto.NewPassword);
        string confirmPassword = rsaHelper.Decrypt(userPassDto.ConfirmPassword);

        if (oldPassword == newPassword)
             throw new  BusException(App.L.R("Error.PasswordSameAsOld"));

        if (!newPassword.Equals(confirmPassword))
        {
             throw new  BusException(App.L.R("Error.InputsDoNotMatch"));
        }

        var curUser = await TableWhere(x => x.Id == App.HttpUser.Id).FirstAsync();
        if (curUser.IsNull())
        {
             throw new  BusException(ValidationError.NotExist());
        }

        if (!BCryptHelper.Verify(oldPassword, curUser.Password))
        {
             throw new  BusException(App.L.R("Error.IncorrectOldPassword"));
        }

        //设置用户密码
        curUser.Password = BCryptHelper.Hash(newPassword);
        curUser.PasswordReSetTime = DateTime.Now;
        var isTrue = await UpdateAsync(curUser);
        if (isTrue>0)
        {
            //清理缓存
            await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserInfoById +
                                        curUser.Id.ToString().ToMd5String16());

            //退出当前用户
            await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.OnlineKey +
                                        App.HttpUser.JwtToken.ToMd5String16());
        }

        return curUser.Id;
    }

    /// <summary>
    /// 修改邮箱
    /// </summary>
    /// <param name="updateUserEmailDto"></param>
    /// <returns></returns>
    public async Task<long> UpdateEmailAsync(UpdateUserEmailDto updateUserEmailDto)
    {
        var curUser = await TableWhere(x => x.Id == App.HttpUser.Id).FirstAsync();
        if (curUser.IsNull())
        {
             throw new  BusException(ValidationError.NotExist());
        }

        var rsaOptions = App.GetOptions<RsaOptions>();
        var rsaHelper = new RsaHelper(rsaOptions.PrivateKey, rsaOptions.PublicKey);
        string password = rsaHelper.Decrypt(updateUserEmailDto.Password);
        if (!BCryptHelper.Verify(password, curUser.Password))
        {
             throw new  BusException(App.L.R("Error.InvalidPassword"));
        }

        var code = await App.Cache.GetAsync<string>(
            GlobalConstants.CachePrefix.EmailCaptcha + updateUserEmailDto.Email.ToMd5String16());
        if (code.IsNullOrEmpty() || !code.Equals(updateUserEmailDto.Code))
        {
             throw new  BusException(App.L.R("Error.InvalidVerificationCode"));
        }

        curUser.Email = updateUserEmailDto.Email;
        var result = await UpdateAsync(curUser);
        return curUser.Id;
    }

    /// <summary>
    /// 更新用户头像
    /// </summary>
    /// <param name="file"></param>
    /// <returns></returns>
    public async Task<long> UpdateAvatarAsync(IFormFile file)
    {
        var curUser = await TableWhere(x => x.Id == App.HttpUser.Id).FirstAsync();
        if (curUser.IsNull())
        {
             throw new  BusException(ValidationError.NotExist());
        }


        var prefix = App.WebHostEnvironment.WebRootPath;
        string avatarName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + IdHelper.NextId() +
                            file.FileName.Substring(Math.Max(file.FileName.LastIndexOf('.'), 0));
        string avatarPath = Path.Combine(prefix, "uploads", "file", "avatar");

        if (!Directory.Exists(avatarPath))
        {
            Directory.CreateDirectory(avatarPath);
        }

        avatarPath = Path.Combine(avatarPath, avatarName);
        await using (var fs = new FileStream(avatarPath, FileMode.CreateNew))
        {
            await file.CopyToAsync(fs);
            fs.Flush();
        }

        string relativePath = Path.GetRelativePath(prefix, avatarPath);
        relativePath = "/" + relativePath.Replace("\\", "/");
        curUser.AvatarPath = relativePath;
        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserInfoById +
                                    curUser.Id.ToString().ToMd5String16());
        var result = await UpdateAsync(curUser);
        return curUser.Id;
    }

    #endregion

    #region 用户缓存

    private async Task ClearUserCache(long userId)
    {
        //清理缓存
        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserInfoById +
                                    userId.ToString().ToMd5String16());
        await App.Cache.RemoveAsync(
            GlobalConstants.CachePrefix.UserPermissionUrls + userId.ToString().ToMd5String16());
        await App.Cache.RemoveAsync(
            GlobalConstants.CachePrefix.UserPermissionRoles + userId.ToString().ToMd5String16());
        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserMenuById +
                                    userId.ToString().ToMd5String16());
        await App.Cache.RemoveAsync(GlobalConstants.CachePrefix.UserDataScopeById +
                                    userId.ToString().ToMd5String16());
    }

    #endregion

    #region 条件模型

    //private async Task<List<IConditionalModel>> GetConditionalModel(UserQueryCriteria userQueryCriteria)
    //{
    //    if (userQueryCriteria.DeptId > 0)
    //    {
    //        var allIds = await _deptService.GetChildIds([userQueryCriteria.DeptId], null);
    //        if (allIds.Any())
    //        {
    //            userQueryCriteria.DeptIdItems = string.Join(",", allIds);
    //        }
    //    }

    //    return userQueryCriteria.ApplyQueryConditionalModel();
    //}

    #endregion
}
