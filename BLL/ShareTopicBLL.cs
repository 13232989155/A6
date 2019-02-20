using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class ShareTopicBLL : Base.BaseBLL<ShareTopicEntity>
    {
        /// <summary>
        /// 获取进行中的列表
        /// </summary>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public List<ShareTopicEntity> UnderwayList()
        {
            return ActionDal.ActionDBAccess.Queryable<ShareTopicEntity>()
                     .Where( it => it.isDel == false && it.startDate <= SqlFunc.GetDate() && SqlFunc.GetDate() <= it.endDate)
                     .OrderBy(it => it.startDate, OrderByType.Desc )
                     .ToList();
        }

        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<ShareTopicEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<ShareTopicEntity> shareTopicEntities = ActionDal.ActionDBAccess.Queryable<ShareTopicEntity>()
                                                  .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.title.Contains(searchString)
                                                     || SqlFunc.ToString(it.shareTopicId).Contains(searchString))
                                                  .OrderBy(it => it.createDate, OrderByType.Desc)
                                                  .ToList()
                                                  .ToPagedList(pageNumber, pageSize);
            return shareTopicEntities;
        }

        /// <summary>
        /// 根据id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShareTopicEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<ShareTopicEntity>().Where(it => it.shareTopicId == id).First();
        }
    }
}
