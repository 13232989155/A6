using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class CaseBLL : Base.BaseBLL<CaseEntity>
    {
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="title"></param>
        /// <param name="coverImage"></param>
        /// <param name="describe"></param>
        /// <param name="tips"></param>
        /// <returns></returns>
        public CaseEntity Create(int userId, string title, string coverImage, string describe, string tips)
        {
            CaseEntity caseEntity = new CaseEntity()
            {
                coverImage = coverImage,
                createDate = DateTime.Now,
                describe = describe,
                integral = 0,
                isDel = false,
                modifyDate = DateTime.Now,
                state = -1,
                tips = tips ?? "",
                title = title,
                userId = userId
            };

            return ActionDal.ActionDBAccess.Insertable(caseEntity).ExecuteReturnEntity();

        }

        /// <summary>
        /// 获取最新的说说和食谱
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CircleResultHelper> CaseAndShareList( int pageNumber = 1, int pageSize = 10, int totalCount = 0, int[] userIdInte = null)
        {
            List<CircleResultHelper> circleResultHelpers = new List<CircleResultHelper>();

            if ( userIdInte != null && userIdInte.Length > 0)
            {
                circleResultHelpers = ActionDal.ActionDBAccess.UnionAll(
                                    ActionDal.ActionDBAccess.Queryable<CaseEntity>()
                                    .Where(it => it.isDel == false)
                                    .In( it => it.userId, userIdInte)
                                    .Select(it => new CircleResultHelper
                                    {
                                        id = it.caseId,
                                        createDate = it.createDate,
                                        type = (int)Entity.TypeEnumEntity.TypeEnum.案例
                                    }),

                                    ActionDal.ActionDBAccess.Queryable<ShareEntity>()
                                    .Where(it => it.isDel == false)
                                    .In(it => it.userId, userIdInte)
                                    .Select(it => new CircleResultHelper
                                    {
                                        id = it.shareId,
                                        createDate = it.createDate,
                                        type = (int)Entity.TypeEnumEntity.TypeEnum.说说
                                    }))
                                    .OrderBy(it => it.createDate, OrderByType.Desc)
                                    .ToPageList(pageNumber, pageSize, ref totalCount);
            }
            else
            {
                circleResultHelpers = ActionDal.ActionDBAccess.UnionAll(
                                    ActionDal.ActionDBAccess.Queryable<CaseEntity>()
                                    .Where(it => it.isDel == false)
                                    .Select(it => new CircleResultHelper
                                    {
                                        id = it.caseId,
                                        createDate = it.createDate,
                                        type = (int)Entity.TypeEnumEntity.TypeEnum.案例
                                    }),

                                    ActionDal.ActionDBAccess.Queryable<ShareEntity>()
                                    .Where(it => it.isDel == false)
                                    .Select(it => new CircleResultHelper
                                    {
                                        id = it.shareId,
                                        createDate = it.createDate,
                                        type = (int)Entity.TypeEnumEntity.TypeEnum.说说
                                    }))
                                    .OrderBy(it => it.createDate, OrderByType.Desc)
                                    .ToPageList(pageNumber, pageSize, ref totalCount);
            }

            List<CaseEntity> caseEntities = SetCaseUser(circleResultHelpers.Where(it => it.type == (int)Entity.TypeEnumEntity.TypeEnum.案例).Select( it => it.id).ToArray());
            caseEntities.ForEach(it =>
            {
                circleResultHelpers.Find(itt => itt.id == it.caseId).caseEntity = it;
            });


            List<ShareEntity> shareEntities = SetShareUser(circleResultHelpers.Where(it => it.type == (int)Entity.TypeEnumEntity.TypeEnum.说说).Select(it => it.id).ToArray());
            shareEntities.ForEach(it =>
            {
                circleResultHelpers.Find(itt => itt.id == it.shareId).shareEntity = it;
            });
            


            return circleResultHelpers;
        }

        /// <summary>
        /// 设置案例的个人信息
        /// </summary>
        /// <param name="idInts"></param>
        /// <returns></returns>
        private List<CaseEntity> SetCaseUser( int[] idInts)
        {
            List<CaseEntity> caseEntities = new List<CaseEntity>();
            if (idInts.Length > 0)
            {
                caseEntities = ActionDal.ActionDBAccess.Queryable<CaseEntity, UserEntity>((c, u) => new object[]
                               {
                                    JoinType.Inner, c.userId == u.userId
                               })
                               .In(c => c.caseId, idInts)
                               .Select((c, u) => new CaseEntity
                               {
                                   caseId = c.caseId,
                                   coverImage = c.coverImage,
                                   createDate = c.createDate,
                                   describe = c.describe,
                                   integral = c.integral,
                                   isDel = c.isDel,
                                   modifyDate = c.modifyDate,
                                   name = u.name,
                                   portrait = u.portrait,
                                   state = c.state,
                                   tips = c.tips,
                                   title = c.title,
                                   userId = c.userId

                               })
                               .ToList();
            }
                return caseEntities;
        }

        /// <summary>
        /// 设置说说的个人信息
        /// </summary>
        /// <param name="idInts"></param>
        /// <returns></returns>
        private List<ShareEntity> SetShareUser(int[] idInts)
        {
            List<ShareEntity> shareEntities = new List<ShareEntity>();
            if (idInts.Length > 0)
            {
                 shareEntities = ActionDal.ActionDBAccess.Queryable<ShareEntity, UserEntity>((s, u) => new object[]
                                    {
                                            JoinType.Inner,s.userId == u.userId
                                    })
                                    .In(s => s.shareId, idInts)
                                    .Select((s, u) => new ShareEntity
                                    {
                                        contents = s.contents,
                                        createDate = s.createDate,
                                        shareId = s.shareId,
                                        img = s.img,
                                        integral = s.integral,
                                        isDel = s.isDel,
                                        modifyDate = s.modifyDate,
                                        name = u.name,
                                        portrait = u.portrait,
                                        shareTopicId = s.shareTopicId,
                                        shareTypeId = s.shareTypeId,
                                        userId = s.userId
                                    })
                                    .ToList();
            }


            return shareEntities;
        }

        /// <summary>
        /// 根据ID返回实体
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public CaseEntity GetById(int caseId)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseEntity, UserEntity>((c, u) => new object[]
                               {
                                    JoinType.Inner, c.userId == u.userId
                               })
                               .Where(c => c.caseId == caseId)
                                .Select((c, u) => new CaseEntity
                                {
                                    caseId = c.caseId,
                                    coverImage = c.coverImage,
                                    createDate = c.createDate,
                                    describe = c.describe,
                                    integral = c.integral,
                                    isDel = c.isDel,
                                    modifyDate = c.modifyDate,
                                    name = u.name,
                                    portrait = u.portrait,
                                    state = c.state,
                                    tips = c.tips,
                                    title = c.title,
                                    userId = c.userId

                                })
                               .First();
        }


        /// <summary>
        /// 获取案列列表
        /// </summary>
        /// <param name="caseTagId"></param>
        /// <param name="userId"></param>
        /// <param name="isDel"></param>
        /// <param name="forbidden"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<CaseEntity> List(int caseTagId = -1, int userId = -1, bool isDel = false, bool forbidden = false, int pageNumber = 1, int pageSize = 10, int totalCount = 0)
        {
            List<CaseEntity> caseEntities = new List<CaseEntity>();

            if (caseTagId > 10000)
            {
                caseEntities = ActionDal.ActionDBAccess.Queryable<CaseTagEntity, CaseTagCorrelationEntity, CaseEntity, UserEntity>((ct, ctc, c, u) => new object[]
                               {
                                    JoinType.Inner, ct.caseTagId == ctc.caseTagId && ct.caseTagId == caseTagId,
                                    JoinType.Inner, ctc.caseId == c.caseId,
                                    JoinType.Inner, c.userId == u.userId
                               })
                                .WhereIF(userId > 10000, (ct, ctc, c, u) => c.userId == userId)
                                .WhereIF(!forbidden, (ct, ctc, c, u) => u.forbidden == false)
                                .WhereIF(!isDel, (ct, ctc, c, u) => c.isDel == false)
                                .OrderBy(c => c.createDate, OrderByType.Desc)
                                .Select((ct, ctc, c, u) => new CaseEntity
                                {
                                    caseId = c.caseId,
                                    coverImage = c.coverImage,
                                    createDate = c.createDate,
                                    describe = c.describe,
                                    integral = c.integral,
                                    isDel = c.isDel,
                                    modifyDate = c.modifyDate,
                                    name = u.name,
                                    portrait = u.portrait,
                                    state = c.state,
                                    tips = c.tips,
                                    title = c.title,
                                    userId = c.userId

                                })
                                .ToPageList( pageNumber, pageSize, ref totalCount);
            }
            else
            {
                caseEntities = ActionDal.ActionDBAccess.Queryable<CaseEntity, UserEntity>( ( c, u) => new object[]
                                {
                                    JoinType.Inner, c.userId == u.userId
                                })
                                .WhereIF(userId > 10000, (c, u) => c.userId == userId)
                                .WhereIF(!forbidden, (c, u) => u.forbidden == false)
                                .WhereIF(!isDel, (c, u) => c.isDel == false)
                                .OrderBy(c => c.createDate, OrderByType.Desc)
                                .Select((c, u) => new CaseEntity
                                {
                                    caseId = c.caseId,
                                    coverImage = c.coverImage,
                                    createDate = c.createDate,
                                    describe = c.describe,
                                    integral = c.integral,
                                    isDel = c.isDel,
                                    modifyDate = c.modifyDate,
                                    name = u.name,
                                    portrait = u.portrait,
                                    state = c.state,
                                    tips = c.tips,
                                    title = c.title,
                                    userId = c.userId

                                })
                                .ToPageList(pageNumber, pageSize, ref totalCount);

            }

            return caseEntities;
        }

        /// <summary>
        /// 返回总条数
        /// </summary>
        /// <param name="caseTagId"></param>
        /// <param name="userId"></param>
        /// <param name="isDel"></param>
        /// <param name="forbidden"></param>
        /// <returns></returns>
        public int Count(int caseTagId = -1, int userId = -1, bool isDel = false, bool forbidden = false)
        {
            int totalItemCount = 0;


            if (caseTagId > 10000)
            {
                totalItemCount = ActionDal.ActionDBAccess.Queryable<CaseTagEntity, CaseTagCorrelationEntity, CaseEntity, UserEntity>((ct, ctc, c, u) => new object[]
                               {
                                    JoinType.Inner, ct.caseTagId == ctc.caseTagId && ct.caseTagId == caseTagId,
                                    JoinType.Inner, ctc.caseId == c.caseId,
                                    JoinType.Inner, c.userId == u.userId
                               })
                                .WhereIF(userId > 10000, (ct, ctc, c, u) => c.userId == userId)
                                .WhereIF(!forbidden, (ct, ctc, c, u) => u.forbidden == false)
                                .WhereIF(!isDel, (ct, ctc, c, u) => c.isDel == false)
                                .OrderBy(c => c.createDate, OrderByType.Desc)
                                .Count();
            }
            else
            {
                totalItemCount = ActionDal.ActionDBAccess.Queryable<CaseEntity, UserEntity>((c, u) => new object[]
                               {
                                    JoinType.Inner, c.userId == u.userId
                               })
                                .WhereIF(userId > 10000, (c, u) => c.userId == userId)
                                .WhereIF(!forbidden, (c, u) => u.forbidden == false)
                                .WhereIF(!isDel, (c, u) => c.isDel == false)
                                .OrderBy(c => c.createDate, OrderByType.Desc)
                                .Count();

            }


            return totalItemCount;
        }

        /// <summary>
        /// 获取案例总数目
        /// </summary>
        /// <param name="userIdInts"></param>
        /// <returns></returns>
        public int CountByUserIdInts(int[] userIdInts)
        {
            int totalItemCount = 0;

            totalItemCount = ActionDal.ActionDBAccess.Queryable<CaseEntity, UserEntity>((c, u) => new object[]
                            {
                                JoinType.Inner, c.userId == u.userId
                            })
                            .Where((c, u) => u.forbidden == false && c.isDel == false)
                            .In( (c, u) => u.userId, userIdInts)
                            .OrderBy(c => c.createDate, OrderByType.Desc)
                            .Count();

            return totalItemCount;
        }

    }
}
