using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BearPlatform.Common.Exception;
using BearPlatform.Common.Extensions;
using BearPlatform.Common.Global;
using BearPlatform.Common.Model;
using BearPlatform.Core;
using BearPlatform.Core.Utils;
using BearPlatform.Entity.Core.Message.Email;
using BearPlatform.IBusiness.Message.Email;
using BearPlatform.Models;
using BearPlatform.Models.Dto.Core.Message.Email;
using BearPlatform.Models.Queries.Common;
using BearPlatform.Models.Queries.Message;
using BearPlatform.SqlSugar;
using BearPlatform.ViewModel.Core.Message.Email;
using BearPlatform.ViewModel.Report.Message.Email.Account;

namespace BearPlatform.Business.Message.Email;

/// <summary>
/// 邮箱账户服务
/// </summary>
public class EmailAccountService : BaseServices<EmailAccount>, IEmailAccountService
{

 


        #region 字段

        #endregion
        #region 构造函数
        public EmailAccountService() : base()
        {
        }
        #endregion
        #region 基础方法
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<EmailAccountDTO>> GetPageAsync(EmailAccountParam param)
        {

            var page = await GetIQueryable().Select(x => new EmailAccountDTO
            {
            }, true).SearchWhere(param).ToPagedResultsAsync(param);
            return page;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<EmailAccountInfo> GetInfoAsync(long id)
        {
            var entity = await GetIQueryable(x => x.Id == id).Select<EmailAccountInfo>().FirstAsync();
            return entity;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(UpdateEmailAccountParam param)
        {
        if (await GetIQueryable(x => x.Email == param.Email).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Email)));
        }

        var model = App.Mapper.MapTo<EmailAccount>(param);
        var result = await AddAsync(model);
        return param.Id;
    }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> UpdateAsync(UpdateEmailAccountParam param)
        {


        var oldEmailAccount = await GetIQueryable(x => x.Id == param.Id).FirstAsync();
        if (oldEmailAccount.IsNull())
        {
            throw new BusException(ValidationError.NotExist(param,
                LanguageKeyConstants.EmailAccount,
                nameof(param.Id)));
        }

        if (oldEmailAccount.Email != param.Email &&
            await TableWhere(j => j.Email == param.Email).AnyAsync())
        {
            throw new BusException(ValidationError.IsExist(param,
                nameof(param.Email)));
        }

        var model = App.Mapper.MapTo<EmailAccount>(param);
        await UpdateAsync(model);
        return model.Id;
    }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(HashSet<long> ids)
        {
            var emailAccounts = await TableWhere(x => ids.Contains(x.Id)).ToListAsync();
            if (emailAccounts.Count < 1)
             {
            throw new BusException(ValidationError.NotExist());
             }

            return await LogicDeleteAsync<EmailAccount>(x => ids.Contains(x.Id));
        }
        #endregion



 
}
