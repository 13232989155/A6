using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{

    ///<summary>
    ///案例标签关联
    ///</summary>
    [SugarTable("t_case_tag_correlation")]
    public partial class CaseTagCorrelationEntity
    {
        public CaseTagCorrelationEntity()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>       
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int caseTagCorrelation { get; set; }

        /// <summary>
        /// Desc:案例ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int caseId { get; set; }

        /// <summary>
        /// Desc:案例标签ID 
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int caseTagId { get; set; }

        /// <summary>
        /// Desc:是否删除
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool isDel { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }

        /// <summary>
        /// Desc:修改时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime modifyDate { get; set; }

    }
}
