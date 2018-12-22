using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class CommentBLL : Base.BaseBLL<CommentEntity>
    {
        /// <summary>
        /// 创建评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public int Create(int userId, int type, int objId, string contents)
        {
            CommentEntity commentEntity = new CommentEntity()
            {
                contents = contents,
                createDate = DateTime.Now,
                img = "",
                isDel = false,
                modifyDate = DateTime.Now,
                objId = objId,
                score = -1,
                type = type,
                userId = userId
            };

            return ActionDal.ActionDBAccess.Insertable(commentEntity).ExecuteCommand();
        }

        /// <summary>
        /// 根据ID返回实体
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public CommentEntity GetById(int commentId)
        {
            return ActionDal.ActionDBAccess.Queryable<CommentEntity>().Where(it => it.commentId == commentId).First();
        }

        /// <summary>
        /// 根据对象ID数组获取评论列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objIdInts"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public IEnumerable<CommentEntity> ListByTypeAndObjIdInts(int type, int[] objIdInts, bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<CommentEntity>()
                    .Where(it => it.type == type)
                    .WhereIF(!isDel, it => it.isDel == false)
                    .In(it => it.objId, objIdInts)
                    .OrderBy(it => it.createDate, SqlSugar.OrderByType.Desc)
                    .ToList();
        }

        /// <summary>
        /// 获取对象评论列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public IEnumerable<CommentEntity> ListByTypeAndObjId(int type, int objId, bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<CommentEntity>()
                    .Where(it => it.type == type && it.objId == objId)
                    .WhereIF(!isDel, it => it.isDel == false)
                    .OrderBy(it => it.createDate, SqlSugar.OrderByType.Desc)
                    .ToList();
        }

        /// <summary>
        /// 获取对象评论列表(包含用户信息)
        /// </summary>
        /// <param name="type"></param>
        /// <param name="objId"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public IEnumerable<CommentEntity> ListUserByTypeAndObjId(int type, int objId, bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<CommentEntity, UserEntity>( (c, u) => new object[]
                    {
                         JoinType.Inner, c.userId == u.userId
                    })
                    .Where((c, u) => c.type == type && c.objId == objId)
                    .WhereIF(!isDel, (c, u) => c.isDel == false)
                    .OrderBy(c => c.createDate, SqlSugar.OrderByType.Desc)
                    .Select( ( c, u) => new CommentEntity
                    {
                        commentId = c.commentId,
                        contents = c.contents,
                        createDate = c.createDate,
                        img = c.img,
                        isDel = c.isDel,
                        modifyDate = c.modifyDate,
                        name = u.name,
                        objId = c.objId,
                        portrait = u.portrait,
                        score = c.score,
                        type = c.type,
                        userId = c.userId
                    })
                    .ToList();
        }

    }
}
