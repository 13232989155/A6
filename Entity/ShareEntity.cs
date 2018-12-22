using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///说说
    ///</summary>
    [SugarTable("t_share")]
    public partial class ShareEntity
    {
        public ShareEntity()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>          
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int shareId { get; set; }

        /// <summary>
        /// Desc:说说类型
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int shareTypeId { get; set; }

        /// <summary>
        /// Desc:话题ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int shareTopicId { get; set; }

        /// <summary>
        /// Desc:用户
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

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

        /// <summary>
        /// Desc:收到的打赏
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

        /// <summary>
        /// 用户姓名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string name { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string portrait { get; set; }

        /// <summary>
        /// 阅读量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int readCount { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int endorseCount { get; set; }

        /// <summary>
        /// 是否已点赞
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool isEndorse { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int commentCount { get; set; }

    }
}
