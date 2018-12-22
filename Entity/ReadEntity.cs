using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///阅读记录
    ///</summary>
    [SugarTable("t_read")]
    public partial class ReadEntity
    {
        public ReadEntity()
        {


        }
        /// <summary>
        /// Desc:ID
        /// Default:
        /// Nullable:False
        /// </summary>    
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int readId { get; set; }

        /// <summary>
        /// Desc:用户ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:被阅读的对象ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int objId { get; set; }

        /// <summary>
        /// Desc:被阅读对象的类型：1 说说；2 案列;
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int type { get; set; }

        /// <summary>
        /// Desc:阅读时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }

    }
}
