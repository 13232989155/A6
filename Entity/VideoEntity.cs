using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///视频
    ///</summary>
    [SugarTable("t_video")]
    public partial class VideoEntity
    {
        public VideoEntity()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>         
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int videoId { get; set; }

        /// <summary>
        /// Desc:视频标题
        /// Default:
        /// Nullable:False
        /// </summary>           

        public string remark { get; set; }

        /// <summary>
        /// Desc:视频地址
        /// Default:
        /// Nullable:False
        /// </summary>          
        public string url { get; set; }

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
