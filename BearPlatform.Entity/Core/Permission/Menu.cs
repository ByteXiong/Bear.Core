using System.Collections.Generic;
using BearPlatform.Common.Enums;
using BearPlatform.Entity.Base;
using BearPlatform.Entity.Core.Permission.Role;
using SqlSugar;
namespace BearPlatform.Entity.Permission;

/// <summary>
/// 系统菜单
/// </summary>
[SugarTable("sys_menu")]
public class Menu : BaseEntity<long>
{  /// <summary>
   /// 权限标识符
   /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Permission { get; set; }
    /// <summary>
    /// 菜单名
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Name { get; set; }


    /// <summary>
    /// 路径
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string PathParam { get; set; }

    /// <summary>
    /// 布局
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public LayoutTypeEnum? Layout { get; set; }

    /// <summary>
    /// 重定向
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Redirect { get; set; }


    /// <summary>
    /// 组件
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Component { get; set; }


    /// <summary>
    /// 父级
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public long? ParentId { get; set; }
    /// <summary>
    /// 类型
    /// </summary>
    public MenuTypeEnum MenuType { get; set; }

    public IconTypeEnum IconType { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public bool Status { get; set; }
    #region RouteMeta


    /// <summary>
    /// 路由标题(可用来作document.title或者菜单的名称)
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Title { get; set; }


    /// <summary>
    /// 缓存页面
    /// </summary>
    public bool KeepAlive { get; set; }
    /// <summary>
    /// 当设置为true时，将不会进行登录验证，也不会进行访问路径的权限验证
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public bool Constant { get; set; }

    /// <summary>
    /// 菜单和面包屑对应的图标
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Icon { get; set; }

    /// <summary>
    /// 使用本地svg作为的菜单和面包屑对应的图标(assets/svg-icon文件夹的的svg文件名)
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string LocalIcon { get; set; }
    /// <summary>
    /// 菜单和面包屑对应的图标的字体大小
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public int IconFontSize { get; set; }


    /// <summary>
    /// 路由顺序，可用于菜单的排序
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// 外链链接
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string Href { get; set; }
    /// <summary>
    /// 是否在菜单中隐藏路线
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public bool HideInMenu { get; set; }


    /// <summary>
    /// 当前路由需要选中的菜单项(用于跳转至不在左侧菜单显示的路由且需要高亮某个菜单的情况)
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public string ActiveMenu { get; set; }

    /// <summary>
    /// 是否支持多个tab页签(默认一个，即相同name的路由会被替换)
    /// </summary>
    public bool MultiTab { get; set; }
    /// <summary>
    /// 如果设置，路线将固定在制表符中，值是固定制表符的顺序
    /// </summary>
    [SugarColumn(IsNullable = true)]
    public int FixedIndexInTab { get; set; }


    public bool Props { get; set; }


    #endregion
    /// <summary>
    /// 父菜单
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(NavigateType.OneToOne, nameof(ParentId))]//设置导航 一对一
    public Menu Parent { get; set; }
    /// <summary>
    /// 子菜单集合
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public List<Menu> Children { get; set; }


    #region 导航
    /// <summary>
    /// 菜单集合
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    [Navigate(typeof(RoleMenu), nameof(RoleMenu.MenuId), nameof(RoleMenu.RoleId))]
    public List<Role> Roles { get; set; }

    #endregion

}
