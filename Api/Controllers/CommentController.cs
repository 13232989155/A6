using System;
using System.Collections.Generic;
using System.Linq;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// 评论
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : BaseController
    {

        private CommentBLL commentBLL = null;

        /// <summary>
        /// 
        /// </summary>
        public CommentController()
        {
            this.commentBLL = new CommentBLL();
        }


        /// <summary>
        /// 获取评论列表
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type">类型*： 1 说说； 2 案例；3 官方案例;</param>
        /// <param name="objId">主键ID*</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult PageListByObjId( [FromForm] string token, [FromForm] int type, [FromForm] int objId, [FromForm] int pageNumber = 1, [FromForm] int pageSize = 10)
        {

            DataResult dr = new DataResult();
            try
            {
                int totalItemCount = commentBLL.CountUserByTypeAndObjId(type, objId);
                if (totalItemCount > 0)
                {
                    List<CommentEntity> commentEntities = commentBLL.ListUserByTypeAndObjId(type, objId, pageNumber: pageNumber, pageSize: pageSize, totalCount: totalItemCount);

                    int[] commentIdInts = commentEntities.Select(it => it.commentId).ToArray();
                    CommentReplyBLL commentReplyBLL = new CommentReplyBLL();

                    List<CommentReplyEntity> commentReplyEntities = commentReplyBLL.ListByCommentIdInts(commentIdInts);

                    if (commentReplyEntities.Count() > 0)
                    {
                        Dictionary<int, List<CommentReplyEntity>> keyValuePairs = commentReplyEntities
                                                                               .GroupBy(it => it.commentId)
                                                                               .Select(it => new
                                                                               {
                                                                                   id = it.Key,
                                                                                   entity = it.ToList()
                                                                               })
                                                                               .ToDictionary(it => it.id, it => it.entity);

                        commentEntities = (from c in commentEntities
                                           join k in keyValuePairs on c.commentId equals k.Key into ck
                                           from ckk in ck.DefaultIfEmpty()
                                           select (new CommentEntity
                                           {
                                               commentId = c.commentId,
                                               commentReplyEntities = ckk.Value,
                                               contents = c.contents,
                                               createDate = c.createDate,
                                               img = c.img,
                                               isDel = c.isDel,
                                               modifyDate = c.modifyDate,
                                               name = c.name,
                                               objId = c.objId,
                                               portrait = c.portrait,
                                               score = c.score,
                                               type = c.type,
                                               userId = c.userId
                                           }))
                                           .ToList();

                    }


                    PageData pageData = new PageData(commentEntities, pageNumber, pageSize, totalItemCount);

                    dr.code = "200";
                    dr.data = pageData;
                }
                else
                {
                    PageData pageData = new PageData(null, pageNumber, pageSize, 0);
                    dr.code = "200";
                    dr.msg = "没有评论";
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
        /// <param name="token">*</param>
        /// <param name="objId">对象ID*</param>
        /// <param name="contents">评论的文字内容*</param>
        /// <param name="type">类型*：1 说说； 2 案例；3 官方案例;</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create([FromForm] string token, [FromForm] int objId, [FromForm] string contents, [FromForm] int type)
        {
            DataResult dr = new DataResult();
            try
            {

                if (objId < 10000 || string.IsNullOrWhiteSpace(contents) || type < 1)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                CommentBLL commentBLL = new CommentBLL();
                int rows = commentBLL.Create(userEntity.userId, type, objId, contents);
                //增加阅读记录
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, type, objId);
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
        /// <param name="token">*</param>
        /// <param name="commentId">评论ID*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete([FromForm] string token, [FromForm] int commentId)
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
        /// <param name="token">*</param>
        /// <param name="commentId">评论ID*</param>
        /// <param name="contents">回复的内容*</param>
        /// <param name="toUserId">被回复的这条评论的所属用户ID*</param>
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
        /// <param name="token">*</param>
        /// <param name="commentReplyId">回复评论ID*</param>
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




    }
}