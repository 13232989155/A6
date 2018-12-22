using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///评论
    ///</summary>
    [SugarTable("t_comment")]
    public partial class CommentEntity
    {
        public CommentEntity()
        {


        }
        /// <summary>
        /// Desc:主键、自增
        /// Default:
        /// Nullable:False
        /// </summary>        
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int commentId { get; set; }

        /// <summary>
        /// Desc:评论的用户id
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:被评论对象的id
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int objId { get; set; }

        /// <summary>
        /// Desc:被评论对象的类型：1 说说； 2 案例；
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int type { get; set; }

        /// <summary>
        /// Desc:评论的内容
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
        /// Desc:评分
        /// Default:-1
        /// Nullable:False
        /// </summary>  
        public int score { get; set; }

        /// <summary>
        /// Desc:图片
        /// Default:-1
        /// Nullable:False
        /// </summary>  
        public string img { get; set; }

        /// <summary>
        /// Desc:评论时间
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
        /// 用户头像
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string portrait { get; set; }

        /// <summary>
        /// 用户姓名
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string name { get; set; }

        /// <summary>
        /// 评论回复列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CommentReplyEntity> commentReplyEntities { set; get; }


    }
}
