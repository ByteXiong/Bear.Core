using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using BearPlatform.Common;
using BearPlatform.Common.Exception;
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
    public class TableFormService : BaseServices<TableForm>, ITableFormService
    {
        readonly ITableFormItemService _tableColumnService;
        public TableFormService(ITableFormItemService tableColumnService )
        {
            _tableColumnService = tableColumnService;
        }


        #region 表头信息设置
        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TableForm> GetTableFormAsync(TableFormParam param)
        {

            param.Tableof = param.Tableof.Trim();
            param.Router = (param.Router ?? "").Trim();
            var sysList = new List<TableFormItem>();
            if (param.Tableof.IsNullOrEmpty()) throw new BusException("表名不能为空");
            else if (!param.ConfigId.IsNullOrEmpty())
            {
                sysList = await GetTableColumnAsync(param.Tableof, param.ConfigId);
            }
            else
            {

                sysList = GetXml(param.Tableof);
            }

            //获取自定义字段
            var entity = await GetIQueryable(x => x.Tableof == param.Tableof && x.Router == param.Router).Includes(x => x.Items).FirstAsync();

            if (entity == null) throw new BusException("暂未创建模型,请创建模型", AppConfig.OK);

            var keys1 = entity?.Items?.Select(x => x.Prop).ToList() ?? new List<string>();
            var keys2 = sysList.Select(x => x.Prop).ToList();
            var keys = keys1.Union(keys2).Distinct().ToList();
            var list = new List<TableFormItem>();
            foreach (var item in keys)
            {
                var model = entity?.Items?.FirstOrDefault(x => x.Prop == item);
                if (model != null)
                {
                    entity.Items.Remove(model);
                }

                model ??= sysList.FirstOrDefault(x => x.Prop == item);
                list.Add(model);
            }
            list.ForEach(x => x.FormId = entity.Id);
            entity.Items = list.OrderBy(x => !x.IsShow ).ThenBy(x => x.Sort).ToList();
            return entity;
        }
        /// <summary>
        /// 添加模型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(UpdateTableFormParam param)
        {
            TableForm model = param.Adapt<TableForm>();
            await AddAsync(model);
            return model.Id;
        }

        /// <summary>
        /// 编辑模型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> UpdateAsync(UpdateTableFormParam param)
        {
            var entity = await GetIQueryable(x => x.Id == param.Id).FirstAsync();
            entity.Tableof = param.Tableof;
            entity.Router = param.Router;
            await UpdateAsync(entity);
            return param.Id;
        }


      
     



        #endregion





        #region 信息获取
        /// <summary>
        /// 表头信息获取
        /// </summary>
        /// <returns></returns>
        public async Task<TableForm> GetFormAsync(TableFormParam param)
        {
            var entity = await GetIQueryable(x => x.Tableof == param.Tableof)
                .Includes(x => x.Items.OrderBy(x => !x.IsShow && string.IsNullOrEmpty(x.Attrs)).ThenBy(x => x.Sort).ToList()).FirstAsync();
            entity?.Items?.ForEach(x => x.Prop = x.Prop?.ToFirstLowerStr());

            return entity;
        }
        #endregion
        #region 私有方法

        /// <summary>
        /// 反射中找到XML
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<TableFormItem> GetXml(string tableName)
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
            var list = new List<TableFormItem>();
            for (int i = 0; i < props.Length; i++)
            {
                MemberInfo prop = props[i];
                var common = xmlCommentHelper.GetFieldOrPropertyComment(prop);
                var model = new TableFormItem()
                {
                    Label = common.Trim(),
                    Prop = prop.Name?.ToFirstLowerStr(),//转小写,
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
        private async Task<List<TableFormItem>> GetTableColumnAsync(string tableName, string configId)
        {
            App.GetService<IUnitOfWork>().GetDbClient().GetConnection(configId).DbMaintenance.GetColumnInfosByTableName(tableName, false);

            var columnView = SugarClient.AsTenant().GetConnection(configId).DbMaintenance.GetColumnInfosByTableName(tableName, false);//true 走缓存 false不走缓存
            var columns = columnView.Select(x => new TableFormItem
            {
                Prop = x.DbColumnName,
                Label = x.ColumnDescription,
            }).ToList();
            return columns;
            #endregion
        }

    }
}
