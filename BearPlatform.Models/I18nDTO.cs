using BearPlatform.Common.Attributes;
using BearPlatform.Common.Enums;
using BearPlatform.Entity;
using BearPlatform.Models.Base;
using BearPlatform.SqlSugar;

namespace BearPlatform.Models;
    #region 查询参数
    public class ApisParam : PageParam
    {
             public VersionEnum? Version { get; set; }
    }
    #endregion

    #region DTO
    /// <summary>
    ///  分页
    /// </summary>
    [AutoMapping(typeof(ApisDTO), typeof(Apis))]
    public class ApisDTO : RootKeyDTO<Guid>
    {
        /// <summary>
        /// 组
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        /// 版本
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 请求方法
        /// </summary>
        public string Method { get; set; }

    }

    /// <summary>
    /// 详情
    /// </summary>
    [AutoMapping(typeof(ApisInfo), typeof(Apis))]
    public class ApisInfo : Apis
    {

    }
    /// <summary>
    /// 更新
    /// </summary>
    [AutoMapping(typeof(UpdateApisParam), typeof(Apis))]
    public class UpdateApisParam : Apis
    {
    }
    #endregion
    public class ApisTreeSelectDTO
    {

        public Guid Id { get; set; }
        public string Label { get; set; }
        public List<ApisTreeSelectDTO> Children { get; set; }
    }

