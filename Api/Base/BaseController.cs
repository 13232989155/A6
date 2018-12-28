using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Api.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : Controller
    {

        /// <summary>
        /// 根据token获取个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected UserEntity GetUserByToken(string token)
        {
            if ( !string.IsNullOrWhiteSpace(token))
            {
                UserBLL userBLL = new UserBLL();
                UserTokenBLL userTokenBLL = new UserTokenBLL();
                UserTokenEntity userTokenEntity = userTokenBLL.GetByToken(token);

                UserEntity userEntity = userBLL.GetById(userTokenEntity.userId);

                return userEntity;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取评论数量
        /// </summary>
        /// <param name="caseEntities"></param>
        /// <returns></returns>
        protected List<CaseEntity> CaseListCommentCountByList(List<CaseEntity> caseEntities)
        {
            if (caseEntities.Count() > 0)
            {
                int[] idInts = caseEntities.Select(it => it.caseId).ToArray();

                CommentBLL commentBLL = new CommentBLL();
                List<CommentEntity> commentEntities = commentBLL.ListByTypeAndObjIdInts( (int)Entity.TypeEnumEntity.TypeEnum.案例, idInts);
                if (commentEntities.Count() > 0)
                {
                    Dictionary<int, int> keyValuePairs = commentEntities
                                                        .GroupBy(it => it.objId)
                                                        .Select(it => new
                                                        {
                                                            id = it.Key,
                                                            count = it.Count()
                                                        })
                                                        .ToDictionary(it => it.id, it => it.count);

                    caseEntities = (from c in caseEntities
                                    join e in keyValuePairs on c.caseId equals e.Key into se
                                    from ses in se.DefaultIfEmpty()
                                    select new CaseEntity
                                    {
                                        caseId = c.caseId,
                                        coverImage = c.coverImage,
                                        createDate = c.createDate,
                                        describe = c.describe,
                                        integral = c.integral,
                                        isDel = c.isDel,
                                        modifyDate = c.modifyDate,
                                        name = c.name,
                                        portrait = c.portrait,
                                        state = c.state,
                                        tips = c.tips,
                                        title = c.title,
                                        userId = c.userId,
                                        commentCount = ses.Value,
                                        endorseCount = c.endorseCount,
                                        isEndorse = c.isEndorse,
                                        readCount = c.readCount
                                    })
                                     .ToList();
                }
            }

            return caseEntities;
        }

        /// <summary>
        /// 获取点赞数量和判断是否点赞
        /// </summary>
        /// <param name="caseEntities"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected List<CaseEntity> CaseListEndorseCountByList(List<CaseEntity> caseEntities, int userId)
        {
            if (caseEntities.Count() > 0)
            {
                int[] idInts = caseEntities.Select(it => it.caseId).ToArray();

                EndorseBLL endorseBLL = new EndorseBLL();
                List<EndorseEntity> endorseEntities = endorseBLL.ListByTypeAndObjIdInts((int)Entity.TypeEnumEntity.TypeEnum.案例, idInts);

                if (endorseEntities.Count() > 0)
                {

                    var userEndorseList = endorseEntities.Where(it => it.userId == userId).ToList();

                    userEndorseList.ToList().ForEach(it =>
                    {
                        caseEntities.First(itt => itt.caseId == it.objId).isEndorse = true;
                    });

                    Dictionary<int, int> keyValuePairs = endorseEntities
                                                        .GroupBy(it => it.objId)
                                                        .Select(it => new
                                                        {
                                                            id = it.Key,
                                                            count = it.Count()
                                                        })
                                                        .ToDictionary(it => it.id, it => it.count);

                    caseEntities = (from c in caseEntities
                                    join e in keyValuePairs on c.caseId equals e.Key into se
                                    from ses in se.DefaultIfEmpty()
                                    select new CaseEntity
                                    {
                                        caseId = c.caseId,
                                        coverImage = c.coverImage,
                                        createDate = c.createDate,
                                        describe = c.describe,
                                        integral = c.integral,
                                        isDel = c.isDel,
                                        modifyDate = c.modifyDate,
                                        name = c.name,
                                        portrait = c.portrait,
                                        state = c.state,
                                        tips = c.tips,
                                        title = c.title,
                                        userId = c.userId,
                                        commentCount = c.commentCount,
                                        endorseCount = ses.Value,
                                        isEndorse = c.isEndorse,
                                        readCount = c.readCount
                                    })
                                     .ToList();

                }

            }

            return caseEntities;
        }

        /// <summary>
        /// 获取评论数量
        /// </summary>
        /// <param name="shareEntities"></param>
        /// <returns></returns>
        protected List<ShareEntity> ShareListCommentCountByList(List<ShareEntity> shareEntities)
        {
            if (shareEntities.Count() > 0)
            {
                int[] shareIdInts = shareEntities.Select(it => it.shareId).ToArray();

                CommentBLL commentBLL = new CommentBLL();
                List<CommentEntity> commentEntities = commentBLL.ListByTypeAndObjIdInts((int)Entity.TypeEnumEntity.TypeEnum.说说, shareIdInts);
                if (commentEntities.Count() > 0)
                {
                    Dictionary<int, int> keyValuePairs = commentEntities
                                                        .GroupBy(it => it.objId)
                                                        .Select(it => new
                                                        {
                                                            id = it.Key,
                                                            count = it.Count()
                                                        })
                                                        .ToDictionary(it => it.id, it => it.count);

                    shareEntities = (from s in shareEntities
                                     join e in keyValuePairs on s.shareId equals e.Key into se
                                     from ses in se.DefaultIfEmpty()
                                     select new ShareEntity
                                     {
                                         contents = s.contents,
                                         createDate = s.createDate,
                                         commentCount = ses.Value,
                                         img = s.img,
                                         integral = s.integral,
                                         modifyDate = s.modifyDate,
                                         shareId = s.shareId,
                                         shareTopicId = s.shareTopicId,
                                         shareTypeId = s.shareTypeId,
                                         userId = s.userId,
                                         isDel = s.isDel,
                                         name = s.name,
                                         portrait = s.portrait,
                                         isEndorse = s.isEndorse,
                                         endorseCount = s.endorseCount,
                                         readCount = s.readCount
                                     })
                                     .ToList();
                }
            }

            return shareEntities;
        }

        /// <summary>
        /// 获取点赞和判断是否已点赞
        /// </summary>
        /// <param name="shareEntities"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        protected List<ShareEntity> ShareListEndorseCountByList(List<ShareEntity> shareEntities, int userId)
        {
            if (shareEntities.Count() > 0)
            {
                int[] shareIdInts = shareEntities.Select(it => it.shareId).ToArray();

                EndorseBLL endorseBLL = new EndorseBLL();
                List<EndorseEntity> endorseEntities = endorseBLL.ListByTypeAndObjIdInts((int)Entity.TypeEnumEntity.TypeEnum.说说, shareIdInts);

                if (endorseEntities.Count() > 0)
                {

                    var userEndorseList = endorseEntities.Where(it => it.userId == userId).ToList();

                    userEndorseList.ToList().ForEach(it =>
                    {
                        shareEntities.First(itt => itt.shareId == it.objId).isEndorse = true;
                    });

                    Dictionary<int, int> keyValuePairs = endorseEntities
                                                        .GroupBy(it => it.objId)
                                                        .Select(it => new
                                                        {
                                                            id = it.Key,
                                                            count = it.Count()
                                                        })
                                                        .ToDictionary(it => it.id, it => it.count);

                    shareEntities = (from s in shareEntities
                                     join e in keyValuePairs on s.shareId equals e.Key into se
                                     from ses in se.DefaultIfEmpty()
                                     select new ShareEntity
                                     {
                                         contents = s.contents,
                                         createDate = s.createDate,
                                         endorseCount = ses.Value,
                                         img = s.img,
                                         integral = s.integral,
                                         modifyDate = s.modifyDate,
                                         shareId = s.shareId,
                                         shareTopicId = s.shareTopicId,
                                         shareTypeId = s.shareTypeId,
                                         userId = s.userId,
                                         isDel = s.isDel,
                                         name = s.name,
                                         portrait = s.portrait,
                                         isEndorse = s.isEndorse,
                                         commentCount = s.commentCount,
                                         readCount = s.readCount

                                     })
                                     .ToList();

                }

            }

            return shareEntities;

        }

    }
}
