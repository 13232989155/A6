using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///评论回复
    ///</summary>
    [SugarTable("t_comment_reply")]
    public partial class CommentReplyEntity
    {
        public CommentReplyEntity()
        {


        }
        /// <summary>
        /// Desc: 主键、自增
        /// Default:
        /// Nullable:False
        /// </summary>          
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int commentReplyId { get; set; }

        /// <summary>
        /// Desc:评论id
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int commentId { get; set; }

        /// <summary>
        /// Desc:被评论的用户id
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int toUserId { get; set; }

        /// <summary>
        /// Desc:评论的用户
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int fromUserId { get; set; }

        /// <summary>
        /// Desc:回复的内容
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string contents { get; set; }

        /// <summary>
        /// Desc:是否删除
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public bool isDel { get; set; }

        /// <summary>
        /// Desc:回复的时间
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
        public string toName { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string fromName { get; set; }
    }
}