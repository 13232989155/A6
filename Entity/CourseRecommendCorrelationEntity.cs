using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{

     ///<summary>
     /// 推荐关联
     ///</summary>
    [SugarTable("t_course_recommend_correlation")]
    public partial class CourseRecommendCorrelationEntity
    {
        public CourseRecommendCorrelationEntity()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>    
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int courseRecommendCorrelationId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int courseId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int courseRecommendId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:DateTime.Now
        /// Nullable:False
        /// </summary>           
        public DateTime createDate { get; set; }

    }
}
