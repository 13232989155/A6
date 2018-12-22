using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///说说类型
    ///</summary>
    [SugarTable("t_share_type")]
    public partial class ShareTypeEntity
    {
        public ShareTypeEntity()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int shareTypeId { get; set; }

        /// <summary>
        /// Desc:类型名称
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
        /// Desc:最后修改的管理员
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
