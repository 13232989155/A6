using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class FansBLL : Base.BaseBLL<FansEntity>
    {
        /// <summary>
        /// 添加关注
        /// </summary>
        /// <param name="userId">要关注的用户</param>
        /// <param name="attentionId">被关注的用户</param>
        /// <returns></returns>
        public int Create(int userId, int attentionId)
        {
            FansEntity fansEntity = new FansEntity()
            {
                attentionId = attentionId,
                createDate = DateTime.Now,
                isDel = false,
                modifyDate = DateTime.Now,
                userId = userId
            };

            return ActionDal.ActionDBAccess.Insertable(fansEntity).ExecuteCommand();
        }

        /// <summary>
        /// 根据userId和attentionId返回对象
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="attentionId"></param>
        /// <returns></returns>
        public FansEntity GetByUserIdAndAttentionId(int userId, int attentionId, bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<FansEntity>()
                    .WhereIF(!isDel, it => it.isDel == false)
                    .Where( it => it.userId == userId && it.attentionId == attentionId)
                    .OrderBy(it => it.createDate, SqlSugar.OrderByType.Desc)
                    .First();
        }

        /// <summary>
        /// 获取粉丝列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<FansUserResult> fansList(int userId)
        {
            List<FansUserResult> fansUserResults = new List<FansUserResult>();
            fansUserResults = ActionDal.ActionDBAccess.Queryable<FansEntity, UserEntity>((f, u) => new object[]
                            {
                                JoinType.Inner, f.userId == u.userId &&
                                f.isDel == false &&
                                u.forbidden == false &&
                                f.attentionId == userId
                            })
                            .OrderBy((f, u) => u.name, OrderByType.Desc)
                            .Select((f, u) => new FansUserResult()
                            {
                                userId = u.userId,
                                name = u.name,
                                gender = u.gender,
                                birthday = u.birthday,
                                portrait = u.portrait,
                                signature = u.signature,
                                attention = false
                            })
                            .ToList();

            return fansUserResults;
        }

        /// <summary>
        /// 查看关注列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<FansUserResult> AttentionList(int userId)
        {
            List<FansUserResult> fansUserResults = new List<FansUserResult>();
            fansUserResults = ActionDal.ActionDBAccess.Queryable<FansEntity, UserEntity>((f, u) => new object[]
                           {
                                JoinType.Inner, f.attentionId == u.userId &&
                                f.isDel == false &&
                                u.forbidden == false &&
                                f.userId == userId
                           })
                            .OrderBy((f, u) => u.name, OrderByType.Desc)
                            .Select((f, u) => new FansUserResult
                            {
                                userId = u.userId,
                                name = u.name,
                                gender = u.gender,
                                birthday = u.birthday,
                                portrait = u.portrait,
                                signature = u.signature,
                                attention = false
                            })
                            .ToList();

            return fansUserResults;
        }
    }
}
