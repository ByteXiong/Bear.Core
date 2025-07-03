using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Entity.Core.Permission.Role;
using BearPlatform.Entity.Core.Permission.User;
using BearPlatform.Entity.Core.System;
using BearPlatform.Entity.Permission;
using BearPlatform.IBusiness;
using BearPlatform.IBusiness.Permission;
using BearPlatform.Models.Permission;
using BearPlatform.Repository.SugarHandler;
using BearPlatform.ViewModel.Core.Permission.Menu;
using SqlSugar;

namespace BearPlatform.Business.Permission;

public class MenuService : BaseServices<Menu>, IMenuService
{
    #region 字段

    private readonly IUserService _userService;

    #endregion

    #region 构造函数

    public MenuService(IUserService userService, ISugarRepository<Menu> repository) : base(repository)
    {
        _userService = userService;
    }

    #endregion

    #region 基础方法
    /// <summary>5
    /// 常量路由
    /// </summary>
    /// <returns></returns>
    [UseCache(Expiration = 120, KeyPrefix = GlobalConstants.CachePrefix.UserMenuConstant)]
    public async Task<List<RouteDTO>> ConstantRoutesAsync()
    {
        var entity = await GetIQueryable(x => x.Constant).ToListAsync();
        return await BuildAsync(entity);
    }


    /// <summary>
    /// 构建前端路由菜单
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns></returns>
    [UseCache(Expiration = 120, KeyPrefix = GlobalConstants.CachePrefix.UserMenuById)]
    public async Task<List<RouteDTO>> BuildTreeAsync(long userId)
    {
        var menuList = await SugarClient
            .Queryable<UserRole, RoleMenu, Menu>((ur, rm, m) => ur.RoleId == rm.RoleId && rm.MenuId == m.Id)
            .Where((ur, rm, m) => ur.UserId == userId && m.MenuType != MenuTypeEnum.Button)
            .OrderBy((ur, rm, m) => m.Order)
            .ClearFilter<ICreateByEntity>()
            .Select((ur, rm, m) => m).Distinct().ToListAsync();
        //var menuListChild = TreeHelper<Menu>.ListToTrees(menuList, "Id", "ParentId", 0);

        return await BuildAsync(menuList);
    }



    /// <summary>
    /// 判断路由是否存在
    /// </summary>
    /// <returns></returns>
    public async Task<bool> IsRouteExistAsync(string name)
    {
        Expression<Func<Menu, bool>> where = x => x.Status;
        where = where.And(x => x.Name == name);
        return await GetIQueryable(where).AnyAsync();
    }


    /// <summary>
    /// 列表
    /// </summary>
    /// <returns></returns>
    public async Task<List<MenuTreeDTO>> GetTreeAsync()
    {
        //排除公共模块
  
        var tree = await GetIQueryable(x => !x.Constant).OrderBy(x => x.Order)
            .Select<MenuTreeDTO>().ToTreeAsync(it => it.Children, it => it.ParentId, null, it => it.Id);
        return tree;
    }

    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<MenuInfo> GetInfoAsync(long id)
    {
        var entity = await GetIQueryable(x => x.Id == id).Select(x => new MenuInfo
        {

            //Title = x.Title,
            //Path = null,
            //Component = null,
            //ComponentName = null,
            //ParentId = null,
            //Sort = 0,
            //Icon = null,
            //Type = (MenuTypeEnum)0,
            //KeepAlive = false,
            //Hidden = false,
            //Redirect = null,
            //AlwaysShow = false,
            //Status = false,
            //IsDeleted = false,
            //Roles = null,
            //Children = null

        }, true).FirstAsync();

        entity.Buttons = await GetIQueryable(x => x.ParentId == id && x.MenuType == MenuTypeEnum.Button).Select(x => new MenuButton
        {
            Id = x.Id,
            Code = x.Name,
            Desc = x.Title,
            ParentId = x.ParentId,
            Status = x.Status
        }).ToListAsync();

        entity.Querys = await GetIQueryable(x => x.ParentId == id && x.MenuType == MenuTypeEnum.Query).Select(x => new MenuQuery
        {
            Id = x.Id,
            Key = x.Name,
            Value = x.Title,
            ParentId = x.ParentId,
            Status = x.Status
        }).ToListAsync();

        return entity;
    }


    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> AddAsync(UpdateMenuParam param)
    {
        var model = App.Mapper.MapTo<Menu>(param);


        await AddAsync(model);

        var addButtons = param.Buttons?.Select(x => new Menu
        {
            MenuType = MenuTypeEnum.Button,
            Title = x.Desc,
            Name = x.Code.ToLower(),
            ParentId = model.Id,
            Status = x.Status
        }).ToList();
        await AddAsync(addButtons);



        var addQuerys = param.Querys?.Select(x => new Menu
        {
            MenuType = MenuTypeEnum.Query,
            Title = x.Key,
            Name = x.Value,
            ParentId = model.Id,
            Status = x.Status
        }).ToList();
        await AddAsync(addQuerys);


        return model.Id;
    }
    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> UpdateAsync(UpdateMenuParam param)
    {
     
        var model = App.Mapper.MapTo<Menu>(param);
        await UpdateAsync(model);

        //新增/编辑
        var buttons = param.Buttons.Select(x => new Menu
        {
            Id = x.Id,
            MenuType = MenuTypeEnum.Button,
            Title = x.Desc,
            Name = x.Code.ToLower(),
            ParentId = param.Id,
            Status = x.Status
        }).ToList();
        //删除
        await  LogicDeleteAsync<Menu>(x => x.MenuType == MenuTypeEnum.Button && x.ParentId == param.Id && buttons.Any(z => z.Id != x.Id));
        await SugarClient.Storageable(buttons).ExecuteCommandAsync();


        //新增/编辑
        var querys = param.Querys.Select(x => new Menu
        {
            Id = x.Id,
            MenuType = MenuTypeEnum.Query,
            Title = x.Key,
            Name = x.Value,
            ParentId = param.Id,
            Status = x.Status
        }).ToList();

        //删除
        await LogicDeleteAsync<Menu>(x => x.MenuType == MenuTypeEnum.Query && x.ParentId == param.Id && querys.Any(z => z.Id != x.Id));
        await SugarClient.Storageable(querys).ExecuteCommandAsync();
        return param.Id;
    }


    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(long[] ids) => await LogicDeleteAsync<Menu>(x => ids.Contains(x.Id));



    /// <summary>
    /// 菜单下拉
    /// </summary>
    /// <returns></returns>
    public async Task<List<RouteTreeSelectDTO>> TreeSelectAsync(MenuTypeEnum[] types) 
    {
        var tree = await GetIQueryable(x => !x.Constant)
            .WhereIF(types != null, x => types.Contains(x.MenuType))
            .OrderBy(x => x.Order)
            .Select<RouteTreeSelectDTO>().ToTreeAsync(it => it.Children, it => it.ParentId, null, it => it.Id);
        return tree;
    }



    #endregion
    #region 私有方法
    /// <summary>
    /// 构建前端路由菜单
    /// </summary>
    /// <param name="menuDtOs"></param>
    /// <returns></returns>
    private async Task<List<RouteDTO>> BuildAsync(List<Menu> menus)

    {
        //查出请求参数有
        var querys = GetIQueryable(x => x.Status && x.MenuType == MenuTypeEnum.Query && menus.Any(y => y.Id == x.ParentId)).Select(x => new MenuQuery
        {
            ParentId = x.ParentId,
            Key = x.Name,
            Status = x.Status,
            Value = x.Title
        }).ToList();

        var entity = menus.Select(x => new RouteDTO()
        {
            Id = x.Id,
            Meta = new RouteMeta
            {
                Icon = x.Icon,
                LocalIcon = x.LocalIcon,
                IconFontSize = x.IconFontSize,
                Order = x.Order,
                Href = x.Href,
                HideInMenu = x.HideInMenu,
                ActiveMenu = x.ActiveMenu,
                MultiTab = x.MultiTab,
                FixedIndexInTab = x.FixedIndexInTab,
                //Query = x.Query,
                KeepAlive = x.KeepAlive,
                Constant = x.Constant,
                Title = x.Title,
                Query = querys.Where(y => y.ParentId == x.Id && !string.IsNullOrEmpty(y.Value) && !string.IsNullOrEmpty(y.Key)).ToList()
            },
            Props = x.Props,
            Name = x.Name,
            Component = Component(x.Layout, x.Component),
            Path = "/" + x.Name + x.PathParam,
            ParentId = x.ParentId,
            //Type = x.Type,
            Redirect = x.Redirect,
            //Status = x.Status,

        }).ToList();


        List<RouteDTO> list = new List<RouteDTO>();

        entity.ForEach(x =>
        {
            x.Children = entity.Where(y => y.ParentId == x.Id).Select(y =>
            {
                y.Path = x.Path + y.Path;
                return y;
            }
                ).ToList();
            if (x.ParentId == null)
            {
                list.Add(x);
            }
        });
        return list;

        //返回组件
        string Component(LayoutTypeEnum? layout, string component)
        {
            var str = string.Empty;
            if (layout != null && !string.IsNullOrEmpty(component))
            {
                str = layout.GetDisplayName() + "$" + component;
            }

            else
                str = layout != null ? layout.GetDisplayName() : component;
            return str;
        }
    }
    #endregion
}
