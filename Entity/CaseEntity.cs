using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///案列
    ///</summary>
    [SugarTable("t_case")]
    public partial class CaseEntity
    {
        public CaseEntity()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>      
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int caseId { get; set; }

        /// <summary>
        /// Desc:用户ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:案例标题
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string title { get; set; }

        /// <summary>
        /// Desc:封面图片
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string coverImage { get; set; }

        /// <summary>
        /// Desc:描述
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string describe { get; set; }

        /// <summary>
        /// Desc:小提示
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string tips { get; set; }

        /// <summary>
        /// Desc:该案例获得的积分
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int integral { get; set; }

        /// <summary>
        /// Desc:状态
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int state { get; set; }

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

        /// <summary>
        /// 步骤列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CaseStepEntity> caseStepEntities { get; set; }
    }
}
