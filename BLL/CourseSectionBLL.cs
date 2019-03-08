using Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class CourseSectionBLL : Base.BaseBLL<CourseSectionEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseSectionEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseSectionEntity>().Where(it => it.courseSectionId == id).First();
        }

        /// <summary>
        /// 获取课程章节列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CourseSectionEntity> ListByCourse(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseSectionEntity>().Where(it => it.courseId == id).ToList();
        }
    }
}
