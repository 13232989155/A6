using Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL
{
    public class CommentReplyBLL : Base.BaseBLL<CommentReplyEntity>
    {
        /// <summary>
        /// 创建评论回复
        /// </summary>
        /// <param name="commentId"></param>
        /// <param name="toUserId"></param>
        /// <param name="fromUserId"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public int Create(int commentId, int toUserId, int fromUserId, string contents)
        {
            CommentReplyEntity commentReplyEntity = new CommentReplyEntity()
            {
                commentId = commentId,
                contents = contents,
                createDate = DateTime.Now,
                fromUserId = fromUserId,
                isDel = false,
                modifyDate = DateTime.Now,
                toUserId = toUserId

            };

            return ActionDal.ActionDBAccess.Insertable(commentReplyEntity).ExecuteCommand();
        }

        /// <summary>
        /// 根据ID返回实体
        /// </summary>
        /// <param name="commentReplyId"></param>
        /// <returns></returns>
        public CommentReplyEntity GetById(int commentReplyId)
        {
            return ActionDal.ActionDBAccess.Queryable<CommentReplyEntity>().Where(it => it.commentReplyId == commentReplyId).First();
        }

        /// <summary>
        /// 获取主评论的评论回复
        /// </summary>
        /// <param name="commentIdInts"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public List<CommentReplyEntity> ListByCommentIdInts(int[] commentIdInts, bool isDel = false)
        {
            return ActionDal.ActionDBAccess.Queryable<CommentReplyEntity, UserEntity, UserEntity>((cr, u1, u2) => new object[]
                    {
                         JoinType.Inner, cr.fromUserId == u1.userId, JoinType.Inner, cr.toUserId == u2.userId
                    })
                    .WhereIF(!isDel, (cr, u1, u2) => cr.isDel == false)
                    .In(cr => cr.commentId, commentIdInts)
                    .OrderBy(cr => cr.createDate)
                    .Select((cr, u1, u2) => new CommentReplyEntity
                    {
                        commentId = cr.commentId,
                        commentReplyId = cr.commentReplyId,
                        contents = cr.contents,
                        createDate = cr.createDate,
                        fromName = u1.name,
                        fromUserId = cr.fromUserId,
                        isDel = cr.isDel,
                        modifyDate = cr.modifyDate,
                        toName = u2.name,
                        toUserId = cr.toUserId
                    })
                    .ToList();
                   
        }
    }
}
