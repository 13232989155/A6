using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{

    ///<summary>
    ///案例步骤
    ///</summary>
    [SugarTable("t_case_step")]
    public partial class CaseStepEntity
    {
        public CaseStepEntity()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>          
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int caseStepId { get; set; }

        /// <summary>
        /// Desc:案件ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int caseId { get; set; }

        /// <summary>
        /// Desc:内容
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string contents { get; set; }

        /// <summary>
        /// Desc:图片
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string img { get; set; }

    }
}