using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///点赞
    ///</summary>
    [SugarTable("t_endorse")]
    public partial class EndorseEntity
    {
        public EndorseEntity()
        {


        }
        /// <summary>
        /// Desc:ID
        /// Default:
        /// Nullable:False
        /// </summary>    
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int endorseId { get; set; }

        /// <summary>
        /// Desc:点赞的用户
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:被点赞的对象
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int objId { get; set; }

        /// <summary>
        /// Desc:被点赞对象的类型:1 说说；
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int type { get; set; }

        /// <summary>
        /// Desc:是否删除
        /// Default:-1
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

