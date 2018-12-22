using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///积分详细表
    ///</summary>
    [SugarTable("t_integral_detail")]
    public partial class IntegralDetailEntity
    {
        public IntegralDetailEntity()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>        
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int integralDetailId { get; set; }

        /// <summary>
        /// Desc:用户ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:获取对象ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int objId { get; set; }

        /// <summary>
        /// Desc:对象类型：1 说说； 2 案列；
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int type { get; set; }

        /// <summary>
        /// Desc:获取的分值
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int integral { get; set; }

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
