using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class CourseBLL : Base.BaseBLL<CourseEntity>
    {
        /// <summary>
        /// 分页数据
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<CourseEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<CourseEntity> courseEntities = ActionDal.ActionDBAccess.Queryable<CourseEntity>()
                                                  .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.tips.Contains(searchString)
                                                     || it.name.Contains(searchString)
                                                     || SqlFunc.ToString(it.courseId).Contains(searchString))
                                                  .OrderBy(it => it.createDate, OrderByType.Desc)
                                                  .Select( it => new CourseEntity
                                                  {
                                                      adminId = it.adminId,
                                                      courseId = it.courseId,
                                                      courseTypeId = it.courseTypeId,
                                                      name = it.name,
                                                      isDel = it.isDel,
                                                      state = it.state,
                                                      createDate = it.createDate,
                                                      modifyDate = it.modifyDate,
                                                      price = it.price
                                                  })
                                                  .ToList()
                                                  .ToPagedList(pageNumber, pageSize);
            return courseEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseEntity>().Where(it => it.courseId == id).First();
        }
    }
}
