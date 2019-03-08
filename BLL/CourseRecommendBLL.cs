using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class CourseRecommendBLL : Base.BaseBLL<CourseRecommendEntity>
    {
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<CourseRecommendEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<CourseRecommendEntity> courseRecommendEntities = ActionDal.ActionDBAccess.Queryable<CourseRecommendEntity>()
                                                .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.name.Contains(searchString)
                                                   || SqlFunc.ToString(it.courseRecommendId).Contains(searchString))
                                                .OrderBy(it => it.createDate, OrderByType.Desc)
                                                .ToList()
                                                .ToPagedList(pageNumber, pageSize);
            return courseRecommendEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseRecommendEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseRecommendEntity>().Where(it => it.courseRecommendId == id).First();
        }
    }
}
