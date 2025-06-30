using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Helper;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.Utils;
using BearPlatform.Entity.Core.Permission;
using BearPlatform.IBusiness.Permission;
using BearPlatform.Models.Dto.Core.Permission;
using BearPlatform.Models.Dto.Core.System.Dict;
using BearPlatform.Models.Dto.Permission;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Permission;
using BearPlatform.Models.Queries.System;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Permission.Dept;
using BearPlatform.ViewModel.Report.Permission;
using NPOI.SS.Formula.Functions;

namespace BearPlatform.Business.Permission;



/// <summary>
/// 组织架构
/// </summary>
public class DeptService : BaseServices<Dept>, IDeptService
{


    #region 字段

    #endregion
    #region 构造函数
    public DeptService() : base()
    {
    }
    #endregion
    #region 基础方法
    /// <summary>
    ///  分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<PagedResults<DeptDTO>> GetPageAsync(DeptParam param)
    {

        var deptId = App.HttpUser.DeptId;
        var data = await GetIQueryable().SearchWhere(param).Select<DeptDTO>().ToTreeAsync(it => it.Children, it => it.ParentId, 0, it => it.Id);
        return new PagedResults<DeptDTO>()
        {
            Data = data,

        };
    }

    /// <summary>
    /// 查询详情
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<DeptInfo> GetInfoAsync(long id)
    {
        var entity = await GetIQueryable(x => x.Id == id).Select<DeptInfo>().FirstAsync();
        return entity;
    }


    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> AddAsync(UpdateDeptParam param)
    {


        if (await GetIQueryable(d => d.Name == param.Name).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Name)));
        }
        Dept dept =
            App.Mapper.MapTo<Dept>(param);
        await AddAsync(dept);

        //重新计算子节点个数
        if (dept.ParentId != 0)
        {
            var model = await GetIQueryable(x => x.Id == dept.ParentId).FirstAsync();
            if (model.IsNotNull())
            {
                var count = await SugarClient.Queryable<Dept>().Where(x => x.ParentId == dept.Id)
                    .CountAsync();
                model.SubCount = count;

                await UpdateAsync(model);
            }
        }
        return dept.Id;
    }

    /// <summary>
    /// 编辑
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<long> UpdateAsync(UpdateDeptParam param)
    {
        var oldUseDept =
           await GetIQueryable(x => x.Id == param.Id).FirstAsync();
        if (oldUseDept.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param,
                LanguageKeyConstants.Dept,
                nameof(param.Id)));
        }

        if (oldUseDept.Name != param.Name &&
            await GetIQueryable(x => x.Name == param.Name).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Name)));
        }

        Dept dept =
            App.Mapper.MapTo<Dept>(param);
        dept.SubCount = oldUseDept.SubCount;
        await UpdateAsync(dept);

        //重新计算子节点个数
        //判断修改前父部门是否与修改后相同  如果相同说明并没有修改上下级部门信息
        if (oldUseDept.ParentId != dept.ParentId)
        {
            if (dept.ParentId != 0)
            {
                var model = await TableWhere(x => x.Id == dept.ParentId).FirstAsync();
                if (model.IsNotNull())
                {
                    var count = await SugarClient.Queryable<Dept>().Where(x => x.ParentId == dept.Id)
                        .CountAsync();
                    model.SubCount = count;
                    await UpdateAsync(model, x => x.SubCount);
                }
            }

            if (oldUseDept.ParentId != 0)
            {
                var model =
                    await GetIQueryable(x => x.Id == oldUseDept.ParentId).FirstAsync();
                if (model.IsNotNull())
                {
                    var count = await SugarClient.Queryable<Dept>().Where(x => x.ParentId == dept.Id)
                        .CountAsync();
                    model.SubCount = count;
                    await UpdateAsync(model, x => x.SubCount);
                }
            }
        }
        return dept.Id;
    }
    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [UseTran]
    public async Task<int> DeleteAsync(HashSet<long> ids)
    {
        //var allIds = await GetChildIds(ids, null);
        var deptList = await GetIQueryable(x => ids.Contains(x.ParentId)).Includes(x => x.Users).Includes(x => x.Roles)
            .ToListAsync();
        if (deptList.Count < 1)
        {
            throw new BusException(ValidationError.NotExist());
        }

        if (deptList.Any(dept => dept.Users != null && dept.Users.Count != 0))
        {
            throw new BusException(ValidationError.DataAssociationExists());
        }

        if (deptList.Any(dept => dept.Roles != null && dept.Roles.Count != 0))
        {
            throw new BusException(ValidationError.DataAssociationExists());
        }


        var pIds = deptList.Select(x => x.ParentId);

        var updateDeptList = await TableWhere(x => pIds.Contains(x.Id)).ToListAsync();

        var isTrue = await LogicDeleteAsync<Dept>(x => ids.Contains(x.Id));

        if (isTrue > 0)
        {
            if (updateDeptList.Any())
            {
                foreach (var d in updateDeptList)
                {
                    var count = await GetIQueryable(x => x.ParentId == d.Id)
                        .CountAsync();
                    d.SubCount = count;
                }

                isTrue = await UpdateAsync(updateDeptList, x => x.SubCount);
            }
        }


        return isTrue;
    }
    #endregion
    #region 扩展接口



    
    ///// <summary>
    ///// 查询同级和父级
    ///// </summary>
    ///// <param name="id"></param>
    ///// <returns></returns>
    //public async Task<List<DeptVo>> QuerySuperiorDeptAsync(long id)
    //{
    //    var deptList = new List<DeptVo>();
    //    var dept = await TableWhere(x => x.Id == id).FirstAsync();
    //    var deptDto = App.Mapper.MapTo<DeptVo>(dept);
    //    var deptVoList = await FindSuperiorAsync(deptDto, new List<DeptVo>());
    //    deptList.AddRange(deptVoList);

    //    deptList = TreeHelper<DeptVo>.ListToTrees(deptList, "Id", "ParentId", 0);

    //    return deptList;
    //}

    ///// <summary>
    ///// 查询
    ///// </summary>
    ///// <param name="id">部门父Id</param>
    ///// <returns></returns>
    //public async Task<List<DeptVo>> QueryByPIdAsync(long id)
    //{
    //    return App.Mapper.MapTo<List<DeptVo>>(await TableWhere(x =>
    //        x.ParentId == id && x.Enabled).ToListAsync());
    //}

    ///// <summary>
    ///// 查询
    ///// </summary>
    ///// <param name="id">部门ID</param>
    ///// <returns></returns>
    //public async Task<DeptSmallVo> QueryByIdAsync(long id)
    //{
    //    return App.Mapper.MapTo<DeptSmallVo>(await TableWhere(x =>
    //        x.Id == id && x.Enabled).FirstAsync());
    //}
    #endregion
}
