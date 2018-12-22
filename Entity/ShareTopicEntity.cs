using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///话题
    ///</summary>
    [SugarTable("t_share_topic")]
    public partial class ShareTopicEntity
    {
        public ShareTopicEntity()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>           
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int shareTopicId { get; set; }

        /// <summary>
        /// Desc:主题
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string title { get; set; }

        /// <summary>
        /// Desc:是否删除
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool isDel { get; set; }

        /// <summary>
        /// Desc:开始时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime startDate { get; set; }

        /// <summary>
        /// Desc:结束时间
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime endDate { get; set; }

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
