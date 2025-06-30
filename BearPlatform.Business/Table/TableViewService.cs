using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Helper;
using BearPlatform.Core;
using BearPlatform.Entity;
using BearPlatform.IBusiness.Table;
using BearPlatform.Models;
using BearPlatform.Repository.UnitOfWork;
using Mapster;
using Newtonsoft.Json;
using SqlSugar;

namespace BearPlatform.Business.Table
{
    /// <summary>
    ///  数据表
    /// </summary>
    public class TableViewService : BaseServices<TableView>, ITableViewService
    {
       
        public TableViewService( ) 
        {
        }


        #region 表头信息设置
        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TableViewInfo> GetEditAsync(TableViewEditParam param)
        {
            param.Tableof= param.Tableof.Trim();
            param.Router = (param.Router??"").Trim();
            var sysList = new List<TableColumn>();
            if (!param.ConfigId.IsNullOrEmpty())
            {
                sysList = await GetTableColumnAsync(param.Tableof, param.ConfigId);
            }
            else
            {

                sysList = GetXml(param.Tableof);
            }

            //获取自定义字段
            var entity = await GetIQueryable(x => x.Tableof == param.Tableof && x.Router ==  param.Router).Select<TableViewInfo>().FirstAsync();

            if (entity == null) {
                entity = new TableViewInfo()
                {
                    Tableof = param.Tableof,
                    Router = param.Router,
                    ConfigId = param.ConfigId,
                    Columns = new List<TableColumn>(),
                };
            }
      

            var keys1 = entity?.Columns?.Select(x => x.Prop).ToList() ?? new List<string>();
            var keys2 = sysList.Select(x => x.Prop).ToList();
            var keys = keys1.Union(keys2).Distinct().ToList();
            var list = new List<TableColumn>();
            int index = 0;
            foreach (var item in keys)
            {
                var model = entity?.Columns?.FirstOrDefault(x => x.Prop == item);
                if (model != null)
                {
                    entity.Columns.Remove(model);
                }

                model ??= sysList.FirstOrDefault(x => x.Prop == item);
                model.Sort = index++;
                list.Add(model);
            }
            entity.Columns = list;
            entity.Sorts = entity.Sorts ?? new Dictionary<string, OrderTypeEnum> { { entity.Columns.FirstOrDefault()?.Prop.ToFirstLowerStr(), OrderTypeEnum.asc } };
            return entity;
        }
        /// <summary>
        /// 编辑模型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [UseTran]
        public async Task<long> SetEditAsync(UpdateTableViewParam param)
        {
            var model = App.Mapper.MapTo<TableView>(param);
            model.Columns = model.Columns.Where(x => !x.IsEditDel).ToList();
            await UpdateAsync(model);
            return param.Id;
        }
        #endregion





        #region 信息获取
        /// <summary>
        /// 表头信息获取
        /// </summary>
        /// <returns></returns>
        public async Task<TableView> GetViewAsync(TableViewEditParam param)
        {
            var entity = await GetIQueryable(x => x.Tableof == param.Tableof).FirstAsync();
            return entity;
        }
        #endregion
        #region 私有方法

        /// <summary>
        /// 反射中找到XML
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<TableColumn> GetXml(string tableName)
        {


            var xmlCommentHelper = new XmlCommentHelper();
            //var xmlFile = AppDomain.CurrentDomain.BaseDirectory + typeName + ".xml";
            //"E:\\MyCode\\LY_WMSCloud\\LY_WMSCloud.Business\\bin\\Debug\\net6.0\\LY_WMSCloud.Models.xml"
            //xmlCommentHelper.Load(new string[] { xmlFile });
            xmlCommentHelper.LoadAll();
            //type
            //var path = $"LY_WMSCloud.Models.{model}";
            //Type type= Type.GetType(path);

            //Assembly assIBll = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "/" + typeName + ".dll");
            ////加载dll后,需要使用dll中某类.
            //Type type = assIBll.GetType($"{typeName}.{tableName}");//获取类名，必须 命名空间+类名 

            Type type = null;
            IList<Assembly> assemblys= RuntimeHelper.GetAllAssemblies();
            foreach (var assembly in assemblys)
            {
                var aa = assembly.GetTypes();
                type = assembly.GetTypes().Where(x => x.Name == tableName).FirstOrDefault();
                if (type != null)
                {
                    break;
                }

            }
            var props = type.GetProperties().Where(p => p.GetCustomAttribute<JsonIgnoreAttribute>() == null).ToArray();


            //entity.Comment = xmlCommentHelper.GetComment($"T:{type.FullName}", "summary");
            var list = new List<TableColumn>();
            for (int i = 0; i < props.Length; i++)
            {
                MemberInfo prop = props[i];
                var common = xmlCommentHelper.GetFieldOrPropertyComment(prop);
                var model = new TableColumn()
                {
                    Label = common.Trim(),
                    Prop = prop.Name?.ToFirstLowerStr(),//转小写,
                    IsEditDel=true,
                    //Sortable = sortable,
                };
                list.Add(model);
            }

            return list;
        }

        /// <summary>
        ///  获取表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private async Task<List<TableColumn>> GetTableColumnAsync(string tableName, string configId)
        {
            App.GetService<IUnitOfWork>().GetDbClient().GetConnection(configId).DbMaintenance.GetColumnInfosByTableName(tableName, false);

            var columnView = SugarClient.AsTenant().GetConnection(configId).DbMaintenance.GetColumnInfosByTableName(tableName, false);//true 走缓存 false不走缓存
            var columns = columnView.Select(x => new TableColumn
            {
                Prop = x.DbColumnName,
                Label = x.ColumnDescription,
            }).ToList();
            return columns;
            #endregion
        }

    }
}
