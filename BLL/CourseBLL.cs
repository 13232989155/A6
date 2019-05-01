using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;
using System.Linq;

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
        /// 获取分页列表
        /// </summary>
        /// <param name="courseTypeId"></param>
        /// <param name="isDel"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CourseEntity> List(int courseTypeId = -1, int pageNumber = 1, int pageSize = 10, int totalCount = 0)
        {
            List<CourseEntity> courseEntities = new List<CourseEntity>();

            courseEntities = ActionDal.ActionDBAccess.Queryable<CourseEntity>()
                            .WhereIF( courseTypeId > 10000, it => it.courseTypeId == courseTypeId)
                            .Where( it => it.isDel == false)
                            .OrderBy( it => it.createDate, OrderByType.Desc)
                            .Select( it => new CourseEntity
                            {
                                courseId = it.courseId,
                                courseTypeId = it.courseTypeId,
                                coverImage = it.coverImage,
                                createDate = it.createDate,
                                name = it.name,
                                price = it.price
                            })
                            .ToPageList(pageNumber, pageSize, ref totalCount);

            return courseEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idInts"></param>
        /// <returns></returns>
        public List<CourseEntity> ListByIdInts(List<int> idInts)
        {
            List<CourseEntity> courseEntities = new List<CourseEntity>();

            courseEntities = ActionDal.ActionDBAccess.Queryable<CourseEntity>()
                          .In(it => it.courseId, idInts)
                          .Where(it => it.isDel == false)
                          .OrderBy(it => it.createDate, OrderByType.Desc)
                          .Select(it => new CourseEntity
                          {
                              courseId = it.courseId,
                              courseTypeId = it.courseTypeId,
                              coverImage = it.coverImage,
                              createDate = it.createDate,
                              name = it.name,
                              price = it.price
                          })
                          .ToList();

            return courseEntities;
        }

        /// <summary>
        /// 获取课程总数
        /// </summary>
        /// <param name="courseTypeId"></param>
        /// <returns></returns>
        public int Count(int courseTypeId = -1)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseEntity>()
                    .WhereIF(courseTypeId > 10000, it => it.courseTypeId == courseTypeId).Count();
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
