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
        /// <param name="type">类型： 1 说说； 2 案例；</param>
        /// <param name="objId">主键ID</param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PageListByObjId( [FromForm] string token, [FromForm] int type, [FromForm] int objId, [FromForm] int pageNumber = 1, [FromForm] int pageSize = 10)
        {

            DataResult dr = new DataResult();
            try
            {

                IEnumerable<CommentEntity> commentEntities = commentBLL.ListUserByTypeAndObjId(type, objId);

                IPagedList<CommentEntity> comments = commentEntities.ToPagedList(pageNumber, pageSize);

                int[] commentIdInts = comments.Select(it => it.commentId).ToArray();
                CommentReplyBLL commentReplyBLL = new CommentReplyBLL();

                IEnumerable<CommentReplyEntity> commentReplyEntities = commentReplyBLL.ListByCommentIdInts(commentIdInts);

                if ( commentReplyEntities.Count() > 0)
                {
                    Dictionary<int, List<CommentReplyEntity>> keyValuePairs = commentReplyEntities
                                                                           .GroupBy(it => it.commentId)
                                                                           .Select(it => new
                                                                           {
                                                                               id = it.Key,
                                                                               entity = it.ToList()
                                                                           })
                                                                           .ToDictionary(it => it.id, it => it.entity);

                    comments = (from c in comments
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
                               .ToPagedList();

                }

                PageData pageData = new PageData(comments);

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



    }
}