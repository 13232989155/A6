using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///案例标签
    ///</summary>
    [SugarTable("t_case_tag")]
    public partial class CaseTagEntity
    {
        public CaseTagEntity()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>       
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int caseTagId { get; set; }

        /// <summary>
        /// Desc:标签名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string name { get; set; }

        /// <summary>
        /// Desc:是否删除
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool isDel { get; set; }

        /// <summary>
        /// Desc:最后修改的管理员ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int adminId { get; set; }

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