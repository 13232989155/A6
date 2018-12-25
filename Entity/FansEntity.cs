using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///关注（粉丝）表
    ///</summary>
    [SugarTable("t_fans")]
    public partial class FansEntity
    {
        public FansEntity()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>       
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int fansId { get; set; }

        /// <summary>
        /// Desc:关注的用户
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:被关注的对象
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int attentionId { get; set; }

        /// <summary>
        /// Desc:是否取消关注
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
        /// 是否已关注
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool concern { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string name { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string portrait { get; set; }

    }
}
