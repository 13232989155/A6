using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class CourseBannerBLL : Base.BaseBLL<CourseBannerEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<CourseBannerEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<CourseBannerEntity> courseBannerEntities = ActionDal.ActionDBAccess.Queryable<CourseBannerEntity>()
                                                  .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.title.Contains(searchString)
                                                     || SqlFunc.ToString(it.courseBannerId).Contains(searchString))
                                                  .OrderBy(it => it.createDate, OrderByType.Desc)
                                                  .ToList()
                                                  .ToPagedList(pageNumber, pageSize);
            return courseBannerEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CourseBannerEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<CourseBannerEntity>().Where(it => it.courseBannerId == id).First();
        }
    }
}
