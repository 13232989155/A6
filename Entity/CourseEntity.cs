using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
     ///<summary>
     ///课程
     ///</summary>
    [SugarTable("t_course")]
    public partial class CourseEntity
    {
        public CourseEntity()
        {


        }
        /// <summary>
        /// Desc:id
        /// Default:
        /// Nullable:False
        /// </summary>      
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int courseId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int courseTypeId { get; set; }

        /// <summary>
        /// Desc:课程标题
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string name { get; set; }

        /// <summary>
        /// Desc:课程描述
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string describe { get; set; }

        /// <summary>
        /// Desc:封面图片
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string coverImage { get; set; }

        /// <summary>
        /// Desc:视频封面
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string coverVideo { get; set; }

        /// <summary>
        /// Desc:老师简介
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string teacherDescribe { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string videoUrl { get; set; }

        /// <summary>
        /// Desc:提示
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string tips { get; set; }

        /// <summary>
        /// Desc:价格
        /// Default:
        /// Nullable:False
        /// </summary>   
        public decimal price { get; set; }

        /// <summary>
        /// Desc:课程状态
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int state { get; set; }

        /// <summary>
        /// Desc:
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool isDel { get; set; }

        /// <summary>
        /// Desc:
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int adminId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }

        /// <summary>
        /// Desc:
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime modifyDate { get; set; }


        /// <summary>
        /// 章节
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CourseSectionEntity> courseSectionEntities { set; get; }

    }
}
