using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
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
        /// 根据ID返回实体
        /// </summary>
        /// <param name="caseId"></param>
        /// <returns></returns>
        public CaseEntity GetById(int caseId)
        {
            return ActionDal.ActionDBAccess.Queryable<CaseEntity>().Where(it => it.caseId == caseId).First();
        }

        /// <summary>
        /// 获取案列列表
        /// </summary>
        /// <param name="caseTagId"></param>
        /// <param name="userId"></param>
        /// <param name="isDel"></param>
        /// <param name="forbidden"></param>
        /// <returns></returns>
        public IEnumerable<CaseEntity> List(int caseTagId = -1, int userId = -1, bool isDel = false, bool forbidden = false)
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
                                .ToList();
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
                                .ToList();

            }

            return caseEntities;
        }
    }
}
