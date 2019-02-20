using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class ShareTypeBLL : Base.BaseBLL<ShareTypeEntity>
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public List<ShareTypeEntity> List( bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<ShareTypeEntity>()
                     .WhereIF(!isDel, it => it.isDel == false)
                     .OrderBy(it => it.createDate)
                     .ToList();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<ShareTypeEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<ShareTypeEntity> shareTypeEntities = ActionDal.ActionDBAccess.Queryable<ShareTypeEntity>()
                                                   .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.name.Contains(searchString)
                                                      || SqlFunc.ToString(it.shareTypeId).Contains(searchString))
                                                   .OrderBy(it => it.createDate, OrderByType.Desc)
                                                   .ToList()
                                                   .ToPagedList(pageNumber, pageSize);
            return shareTypeEntities;
        }

        /// <summary>
        /// 根据id返回实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShareTypeEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<ShareTypeEntity>().Where(it => it.shareTypeId == id).First();
        }
    }
}
