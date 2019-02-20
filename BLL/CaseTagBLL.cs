using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class CaseTagBLL : Base.BaseBLL<CaseTagEntity>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public List<CaseTagEntity> List( bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseTagEntity>()
                    .WhereIF(!isDel, it => it.isDel == false)
                    .OrderBy(it => it.createDate, SqlSugar.OrderByType.Desc)
                    .ToList();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<CaseTagEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<CaseTagEntity> caseTagEntities = ActionDal.ActionDBAccess.Queryable<CaseTagEntity>()
                                                   .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.name.Contains(searchString)
                                                      || SqlFunc.ToString(it.caseTagId).Contains(searchString))
                                                   .OrderBy(it => it.createDate, OrderByType.Desc)
                                                   .ToList()
                                                   .ToPagedList(pageNumber, pageSize);
            return caseTagEntities;
        }

        /// <summary>
        /// 根据id返回实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CaseTagEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseTagEntity>().Where(it => it.caseTagId == id).First();
        }
    }
}
