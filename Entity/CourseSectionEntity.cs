using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entity
{
    ///<summary>
    ///课程章节
    ///</summary>
    [SugarTable("t_course_section")]
    public partial class CourseSectionEntity
    {
        public CourseSectionEntity()
        {


        }
        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int courseSectionId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:-1
        /// Nullable:False
        /// </summary>           
        public int courseId { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string title { get; set; }

        /// <summary>
        /// Desc:
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string videoUrl { get; set; }

    }
}

