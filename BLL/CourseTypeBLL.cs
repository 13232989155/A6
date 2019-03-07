using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class CourseTypeBLL : Base.BaseBLL<CourseTypeEntity>
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<CourseTypeEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<CourseTypeEntity> courseTypeEntities = ActionDal.ActionDBAccess.Queryable<CourseTypeEntity>()
                                                   .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.name.Contains(searchString)
                                                      || SqlFunc.ToString(it.courseTypeId).Contains(searchString))
                                                   .OrderBy(it => it.createDate, OrderByType.Desc)
                                                   .ToList()
                                                   .ToPagedList(pageNumber, pageSize);
            return courseTypeEntities;
        }

        /// <summary>
        /// 根据id返回实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseTypeEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseTypeEntity>().Where(it => it.courseTypeId == id).First();
        }
    }
}
