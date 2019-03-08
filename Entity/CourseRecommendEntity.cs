using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///课程推荐
    ///</summary>
    [SugarTable("t_course_recommend")]
    public partial class CourseRecommendEntity
    {
        public CourseRecommendEntity()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>          
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int courseRecommendId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string name { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>        
        public int adminId{ get;set; }

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
        /// 推荐课程列表
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<CourseRecommendCorrelationEntity> courseRecommendCorrelationEntities { set; get; }

    }
}
