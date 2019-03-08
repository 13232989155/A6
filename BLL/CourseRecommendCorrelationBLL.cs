using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class CourseRecommendCorrelationBLL : Base.BaseBLL<CourseRecommendCorrelationEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="courseRecommendId"></param>
        /// <returns></returns>
        public List<CourseRecommendCorrelationEntity> ListByCourseRecommendId(int courseRecommendId)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseRecommendCorrelationEntity>()
                                .Where(it => it.courseRecommendId == courseRecommendId)
                                .ToList();
        }
    }
}
