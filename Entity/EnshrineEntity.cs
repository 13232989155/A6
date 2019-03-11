using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///用户收藏
    ///</summary>
    [SugarTable("t_enshrine")]
    public partial class EnshrineEntity
    {
        public EnshrineEntity()
        {


        }
        /// <summary>
        /// Desc:主键、自增
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int enshrineId { get; set; }

        /// <summary>
        /// Desc:用户id
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:收藏类型
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int typeId { get; set; }

        /// <summary>
        /// Desc:收藏实体id
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int objId { get; set; }

        /// <summary>
        /// Desc:收藏时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }

        /// <summary>
        /// 案例
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public CaseEntity caseEntity { set; get; }

        /// <summary>
        /// 官方案例
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public CaseOfficialEntity caseOfficialEntity { set; get; }

    }
}
