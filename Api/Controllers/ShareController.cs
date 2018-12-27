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
        /// <param name="token">*</param>
        /// <param name="contents">说说文字内容*</param>
        /// <param name="shareTypeId">说说类型*</param>
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
        /// <param name="token">*</param>
        /// <param name="shareId">说说ID*</param>
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
        /// 根据类型获取说说分页数据
        /// </summary>
        /// <param name="token">*</param>
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
                int totalItemCount = shareBLL.Count(shareTypeId, shareTopicId, userId: userId);
                List<ShareEntity> shareEntities = shareBLL.List(shareTypeId, shareTopicId, userId:userId, pageNumber:pageNumber, pageSize:pageSize, totalCount:totalItemCount);
                UserEntity userEntity = this.GetUserByToken(token);

                shareEntities = ShareListEndorseCountByList(shareEntities, userEntity.userId);

                shareEntities = ShareListCommentCountByList(shareEntities);

                PageData pageData = new PageData(shareEntities, pageNumber, pageSize, totalItemCount);

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
        /// 根据说说ID获取详细内容
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="shareId">*</param>
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
                List<EndorseEntity> endorseEntities = endorseBLL.ListByTypeAndObjId(shareType, shareEntity.shareId);

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