using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using X.PagedList;

namespace BLL
{
    public class CaseOfficialBLL : Base.BaseBLL<CaseOfficialEntity>
    {
        /// <summary>
        /// 分页列表
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <returns></returns>
        public IPagedList<CaseOfficialEntity> AdminPageList(int pageNumber, int pageSize, string searchString)
        {
            IPagedList<CaseOfficialEntity> caseOfficialEntities = ActionDal.ActionDBAccess.Queryable<CaseOfficialEntity>()
                                                  .WhereIF(!string.IsNullOrWhiteSpace(searchString), it => it.title.Contains(searchString)
                                                    || it.contents.Contains(searchString)
                                                    || SqlFunc.ToString(it.caseOfficialId).Contains(searchString))
                                                  .OrderBy(it => it.createDate, OrderByType.Desc)
                                                  .ToList()
                                                  .ToPagedList(pageNumber, pageSize);
            return caseOfficialEntities;
        }

        /// <summary>
        /// 根据ID返回实体
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public CaseOfficialEntity GetById(int caseOfficialId)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseOfficialEntity, UserEntity>((c, u) => new object[]
                               {
                                    JoinType.Inner, c.userId == u.userId
                               })
                               .Where(c => c.caseOfficialId == caseOfficialId)
                                .Select((c, u) => new CaseOfficialEntity
                                {
                                    caseOfficialId = c.caseOfficialId,
                                    coverImage = c.coverImage,
                                    createDate = c.createDate,
                                    integral = c.integral,
                                    isDel = c.isDel,
                                    modifyDate = c.modifyDate,
                                    name = u.name,
                                    portrait = u.portrait,
                                    state = c.state,
                                    contents = c.contents,
                                    title = c.title,
                                    userId = c.userId

                                })
                               .First();
        }

        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="isDel"></param>
        /// <param name="forbidden"></param>
        /// <returns></returns>
        public int Count(int userId = -1, bool isDel = false, bool forbidden = false)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseOfficialEntity, UserEntity>((s, u) => new object[]
                  {
                         JoinType.Inner,s.userId == u.userId
                  })
                   .WhereIF(userId > 10000, (s, u) => s.userId == userId)
                   .WhereIF(!forbidden, (s, u) => u.forbidden == false)
                   .WhereIF(!isDel, (s, u) => s.isDel == false)
                   .OrderBy(s => s.createDate, SqlSugar.OrderByType.Desc)
                   .Count();
        }

        /// <summary>
        /// 获取总条数
        /// </summary>
        /// <param name="userIdInts"></param>
        /// <returns></returns>
        public int CountByUserIdInts(int[] userIdInts)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseOfficialEntity, UserEntity>((s, u) => new object[]
                  {
                         JoinType.Inner,s.userId == u.userId
                  })
                  .Where((s, u) => s.isDel == false && u.forbidden == false)
                  .In((s, u) => u.userId, userIdInts)
                   .OrderBy(s => s.createDate, SqlSugar.OrderByType.Desc)
                   .Count();
        }

    }
}
