using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Api.Controllers
{
    /// <summary>
    /// 圈子说说
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShareController : BaseController
    {

        private ShareBLL shareBLL = null;

        /// <summary>
        /// 阅读、点赞、积分记录的type值
        /// </summary>
        private readonly int shareType = 1;

        /// <summary>
        /// 
        /// </summary>
        public ShareController()
        {
            this.shareBLL = new ShareBLL();
        }



        /// <summary>
        /// 创建说说
        /// </summary>
        /// <param name="token"></param>
        /// <param name="contents">说说文字内容</param>
        /// <param name="shareTypeId">说说类型</param>
        /// <param name="img">照片json数组</param>
        /// <param name="shareTopicId">话题ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create([FromForm] string token, [FromForm] string contents, [FromForm] int shareTypeId, [FromForm] string img = "", [FromForm] int shareTopicId = -1)
        {
            DataResult dr = new DataResult();
            try
            {

                if (string.IsNullOrWhiteSpace(contents) || shareTypeId == 0)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                int rows = shareBLL.Create(userEntity.userId, contents, img, shareTypeId, shareTopicId);

                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);
        }

        /// <summary>
        /// 删除说说
        /// </summary>
        /// <param name="token"></param>
        /// <param name="shareId">说说ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete([FromForm] string token, [FromForm] int shareId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (shareId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }


                ShareEntity shareEntity = shareBLL.GetById(shareId);

                if (shareEntity == null)
                {
                    dr.code = "201";
                    dr.msg = "不存在该说说";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);
                if (shareEntity.userId != userEntity.userId)
                {
                    dr.code = "201";
                    dr.msg = "不是该用户的说说";
                    return Json(dr);
                }

                shareEntity.isDel = true;
                shareEntity.modifyDate = DateTime.Now;

                int rows = shareBLL.ActionDal.ActionDBAccess.Updateable(shareEntity).ExecuteCommand();

                //增加阅读记录
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, shareType, shareId);

                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);
        }

        /// <summary>
        /// 阅读说说
        /// </summary>
        /// <param name="token"></param>
        /// <param name="shareId">说说ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Read([FromForm] string token, [FromForm] int shareId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (shareId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                ReadBLL readBLL = new ReadBLL();
                int rows = readBLL.Create(userEntity.userId, shareType, shareId);

                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);

        }

        /// <summary>
        /// 点赞说说
        /// </summary>
        /// <param name="token"></param>
        /// <param name="shareId">说说ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Endorse([FromForm] string token, [FromForm] int shareId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (shareId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                EndorseBLL endorseBLL = new EndorseBLL();
                int rows = endorseBLL.Create(userEntity.userId, shareType, shareId);
                //增加阅读记录
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, shareType, shareId);
                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);

        }

        /// <summary>
        /// 取消点赞
        /// </summary>
        /// <param name="token"></param>
        /// <param name="shareId">说说ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CancelEndorse([FromForm] string token, [FromForm] int shareId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (shareId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);


                EndorseBLL endorseBLL = new EndorseBLL();
                EndorseEntity endorseEntity = endorseBLL.GetUserIdAndTypeAndObjId(userEntity.userId, shareType, shareId);

                endorseEntity.isDel = true;
                endorseEntity.modifyDate = DateTime.Now;

                int rows = endorseBLL.ActionDal.ActionDBAccess.Updateable(endorseEntity).ExecuteCommand();
                //增加阅读记录
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, shareType, shareId);
                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);

        }


        /// <summary>
        /// 打赏
        /// </summary>
        /// <param name="token"></param>
        /// <param name="shareId">说说ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Sponsor([FromForm] string token, [FromForm] int shareId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (shareId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                if (userEntity.integral < 1)
                {
                    dr.code = "201";
                    dr.msg = "积分不足";
                    return Json(dr);
                }

                ShareEntity shareEntity = shareBLL.GetById(shareId);
                if (shareEntity.isDel)
                {
                    dr.code = "201";
                    dr.msg = "说说已被删除";
                    return Json(dr);
                }
                if (shareEntity.userId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "无主说说";
                    return Json(dr);
                }

                var result = shareBLL.ActionDal.ActionDBAccess.Ado.UseTran(() =>
                {
                    //积分减1
                    userEntity.integral = userEntity.integral - 1;
                    var rows1 = shareBLL.ActionDal.ActionDBAccess.Updateable(userEntity).ExecuteCommand();
                    IntegralDetailEntity integralDetailEntity = new IntegralDetailEntity()
                    {
                        createDate = DateTime.Now,
                        integral = -1,
                        isDel = false,
                        modifyDate = DateTime.Now,
                        objId = shareId,
                        type = shareType,
                        userId = userEntity.userId
                    };
                    var rows2 = shareBLL.ActionDal.ActionDBAccess.Insertable(integralDetailEntity).ExecuteCommand();

                    //积分加1
                    UserEntity user = shareBLL.ActionDal.ActionDBAccess.Queryable<UserEntity>().Where(it => it.userId == shareEntity.userId).First();
                    user.integral = user.integral + 1;
                    var rows3 = shareBLL.ActionDal.ActionDBAccess.Updateable(user).ExecuteCommand();
                    IntegralDetailEntity integralDetail = new IntegralDetailEntity()
                    {
                        createDate = DateTime.Now,
                        integral = 1,
                        isDel = false,
                        modifyDate = DateTime.Now,
                        objId = shareId,
                        type = shareType,
                        userId = user.userId
                    };
                    var rows4 = shareBLL.ActionDal.ActionDBAccess.Insertable(integralDetail).ExecuteCommand();

                    shareEntity.integral = shareEntity.integral + 1;
                    var rows5 = shareBLL.ActionDal.ActionDBAccess.Updateable(shareEntity).ExecuteCommand();
                });
                //增加阅读记录
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, shareType, shareId);
                if (result.IsSuccess)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);

        }



        /// <summary>
        /// 评论
        /// </summary>
        /// <param name="token"></param>
        /// <param name="shareId">说说ID</param>
        /// <param name="contents">评论的文字内容</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateCommnent([FromForm] string token, [FromForm] int shareId, [FromForm] string contents)
        {
            DataResult dr = new DataResult();
            try
            {

                if (shareId < 10000 || string.IsNullOrWhiteSpace(contents))
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                CommentBLL commentBLL = new CommentBLL();
                int rows = commentBLL.Create(userEntity.userId, shareType, shareId, contents);
                //增加阅读记录
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, shareType, shareId);
                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);

        }


        /// <summary>
        /// 删除评论
        /// </summary>
        /// <param name="token"></param>
        /// <param name="commentId">评论ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCommnent([FromForm] string token, [FromForm] int commentId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (commentId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                CommentBLL commentBLL = new CommentBLL();
                CommentEntity commentEntity = commentBLL.GetById(commentId);

                if (userEntity.userId != commentEntity.userId)
                {
                    dr.code = "201";
                    dr.msg = "不是你的评论";
                    return Json(dr);
                }

                commentEntity.isDel = true;
                commentEntity.modifyDate = DateTime.Now;
                int rows = commentBLL.ActionDal.ActionDBAccess.Updateable(commentEntity).ExecuteCommand();
                //增加阅读记录
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, shareType, commentEntity.objId);
                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);

        }

        /// <summary>
        /// 回复评论
        /// </summary>
        /// <param name="token"></param>
        /// <param name="commentId">评论ID</param>
        /// <param name="contents">回复的内容</param>
        /// <param name="toUserId">被回复的这条评论的所属用户ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult CreateCommentReply([FromForm] string token, [FromForm] int commentId, [FromForm] string contents, [FromForm] int toUserId)
        {

            DataResult dr = new DataResult();
            try
            {
                if (commentId < 10000 || string.IsNullOrWhiteSpace(contents) || toUserId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                CommentReplyBLL commentReplyBLL = new CommentReplyBLL();
                int rows = commentReplyBLL.Create(commentId, toUserId, userEntity.userId, contents);
                //增加阅读记录
                CommentBLL commentBLL = new CommentBLL();
                CommentEntity commentEntity = commentBLL.GetById(commentId);
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, shareType, commentEntity.objId);
                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);

        }


        /// <summary>
        /// 删除回复评论
        /// </summary>
        /// <param name="token"></param>
        /// <param name="commentReplyId">回复评论ID</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteCommentReply([FromForm] string token, [FromForm] int commentReplyId)
        {

            DataResult dr = new DataResult();
            try
            {
                if (commentReplyId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                CommentReplyBLL commentReplyBLL = new CommentReplyBLL();
                CommentReplyEntity commentReplyEntity = commentReplyBLL.GetById(commentReplyId);

                if (commentReplyEntity.fromUserId != userEntity.userId)
                {
                    dr.code = "201";
                    dr.msg = "不是你的评论";
                    return Json(dr);
                }
                commentReplyEntity.isDel = true;
                commentReplyEntity.modifyDate = DateTime.Now;
                int rows = commentReplyBLL.ActionDal.ActionDBAccess.Updateable(commentReplyEntity).ExecuteCommand();
                //增加阅读记录
                CommentBLL commentBLL = new CommentBLL();
                CommentEntity commentEntity = commentBLL.GetById(commentReplyEntity.commentId);
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, shareType, commentEntity.objId);
                if (rows > 0)
                {
                    dr.code = "200";
                    dr.msg = "成功";
                }
                else
                {
                    dr.code = "201";
                    dr.msg = "失败";
                }
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);

        }

        /// <summary>
        /// 根据类型获取说说分页数据
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId"></param>
        /// <param name="shareTypeId"></param>
        /// <param name="shareTopicId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PageList([FromForm] string token, [FromForm] int userId = -1, [FromForm] int shareTypeId = -1, [FromForm] int shareTopicId = -1, [FromForm] int pageNumber = 1, [FromForm] int pageSize = 10)
        {
            DataResult dr = new DataResult();
            try
            {

                IEnumerable<ShareEntity> shareEntities = shareBLL.List(shareTypeId, shareTopicId, userId:userId);
                UserEntity userEntity = this.GetUserByToken(token);

                IPagedList<ShareEntity> shares = shareEntities.ToList().ToPagedList(pageNumber, pageSize);
                shares = ListEndorseByList(shares, userEntity.userId);

                shares = ListCommentByList(shares);

                PageData pageData = new PageData(shares);

                dr.code = "200";
                dr.data = pageData;
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);
        }

        /// <summary>
        /// 获取评论数量
        /// </summary>
        /// <param name="shareEntities"></param>
        /// <returns></returns>
        private IPagedList<ShareEntity> ListCommentByList(IPagedList<ShareEntity> shareEntities)
        {
            if (shareEntities.Count() > 0)
            {
                int[] shareIdInts = shareEntities.Select(it => it.shareId).ToArray();

                CommentBLL commentBLL = new CommentBLL();
                IEnumerable<CommentEntity> commentEntities = commentBLL.ListByTypeAndObjIdInts(shareType, shareIdInts);
                if (commentEntities.Count() > 0)
                {
                    Dictionary<int, int> keyValuePairs = commentEntities
                                                        .GroupBy(it => it.objId)
                                                        .Select(it => new
                                                        {
                                                            id = it.Key,
                                                            count = it.Count()
                                                        })
                                                        .ToDictionary( it => it.id, it => it.count);

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
                                         isEndorse = s.isEndorse
                                     })
                                     .ToPagedList();
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
        private IPagedList<ShareEntity> ListEndorseByList(IPagedList<ShareEntity> shareEntities, int userId)
        {
            if (shareEntities.Count() > 0)
            {
                int[] shareIdInts = shareEntities.Select(it => it.shareId).ToArray();

                EndorseBLL endorseBLL = new EndorseBLL();
                IEnumerable<EndorseEntity> endorseEntities = endorseBLL.ListByTypeAndObjIdInts(shareType, shareIdInts);

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
                                         isEndorse = s.isEndorse
                                     })
                                     .ToPagedList();

                }

            }

            return shareEntities;

        }

        /// <summary>
        /// 根据说说ID获取详细内容
        /// </summary>
        /// <param name="token"></param>
        /// <param name="shareId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetById( [FromForm] string token, [FromForm] int shareId)
        {
            DataResult dr = new DataResult();
            try
            {
                ShareEntity shareEntity = shareBLL.GetById(shareId);
                UserEntity userEntity = this.GetUserByToken(token);

                CommentBLL commentBLL = new CommentBLL();
                shareEntity.commentCount = commentBLL.ListByTypeAndObjId(shareType, shareEntity.shareId).Count();

                EndorseBLL endorseBLL = new EndorseBLL();
                IEnumerable<EndorseEntity> endorseEntities = endorseBLL.ListByTypeAndObjId(shareType, shareEntity.shareId);

                shareEntity.endorseCount = endorseEntities.Count();
                if ( endorseEntities.ToList().Exists( it => it.userId == userEntity.userId))
                {
                    shareEntity.isEndorse = true;
                }

                dr.code = "200";
                dr.data = shareEntity;
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);
        }


    }
}