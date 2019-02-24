using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class VideoBLL : Base.BaseBLL<VideoEntity>
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<VideoEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {

            IPagedList<VideoEntity> videoEntities = ActionDal.ActionDBAccess.Queryable<VideoEntity>()
                                                   .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.remark.Contains(searchString)
                                                      || SqlFunc.ToString(it.videoId).Contains(searchString))
                                                   .OrderBy(it => it.createDate, OrderByType.Desc)
                                                   .ToList()
                                                   .ToPagedList(pageNumber, pageSize);
            return videoEntities;
        }

        /// <summary>
        /// 根据id获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public VideoEntity GetById(int id)
        {
            return ActionDal.ActionDBAccess.Queryable<VideoEntity>().Where(it => it.videoId == id).First();
        }
    }
}
