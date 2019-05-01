using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class TeacherBLL : Base.BaseBLL<TeacherEntity>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<TeacherEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<TeacherEntity> teacherEntities = ActionDal.ActionDBAccess.Queryable<TeacherEntity>()
                                                  .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.name.Contains(searchString)
                                                    || SqlFunc.ToString(it.teacherId).Contains(searchString))
                                                  .OrderBy(it => it.createDate, OrderByType.Desc)
                                                  .Select(it => new TeacherEntity
                                                  {
                                                      teacherId = it.teacherId,
                                                      createDate = it.createDate,
                                                      modifyDate = it.modifyDate,
                                                      name = it.name,
                                                      adminId = it.adminId,
                                                      isDel = it.isDel
                                                  })
                                                  .ToList()
                                                  .ToPagedList(pageNumber, pageSize);
            return teacherEntities;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TeacherEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<TeacherEntity>().InSingle(id);
        }
    }
}
