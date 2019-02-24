using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    /// <summary>
    /// 官方案例
    /// </summary>
    [SugarTable("t_case_official")]
    public partial class CaseOfficialEntity
    {
        public CaseOfficialEntity()
        {


        }
        /// <summary>
        /// Desc:主键ID
        /// Default:
        /// Nullable:False
        /// </summary>      
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int caseOfficialId { get; set; }

        /// <summary>
        /// Desc:用户ID
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int userId { get; set; }

        /// <summary>
        /// Desc:案例标题
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string title { get; set; }

        /// <summary>
        /// Desc:封面图片
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string coverImage { get; set; }

        /// <summary>
        /// Desc:内容
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string contents { get; set; }

        /// <summary>
        /// Desc:该案例获得的积分
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int integral { get; set; }

        /// <summary>
        /// Desc:状态
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int state { get; set; }

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

        /// <summary>
        /// 标签列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CaseTagEntity> caseTagEntities { get; set; }
    }
}
