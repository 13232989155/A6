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

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FansController : BaseController
    {

        /// <summary>
        /// 关注
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="userId">要关注的用户的ID*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create([FromForm] string token, [FromForm] int userId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (userId < 10000 || userId < 1)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                if (userEntity.userId == userId)
                {

                    dr.code = "201";
                    dr.msg = "不能关注自己";
                    return Json(dr);
                }

                FansBLL fansBLL = new FansBLL();

                FansEntity fansEntity = fansBLL.GetByUserIdAndAttentionId(userEntity.userId, userId);
                if (fansEntity != null)
                {
                    dr.code = "201";
                    dr.msg = "已关注关注";
                    return Json(dr);
                }

                int rows = fansBLL.Create( userEntity.userId, userId);

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
        /// 取消关注
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="userId">要关注的用户的ID*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete([FromForm] string token, [FromForm] int userId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (userId < 10000 || userId < 1)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                FansBLL fansBLL = new FansBLL();
                FansEntity fansEntity = fansBLL.GetByUserIdAndAttentionId(userEntity.userId, userId);

                if ( fansEntity == null)
                {
                    dr.code = "201";
                    dr.msg = "不存在关注";
                    return Json(dr);
                }

                fansEntity.isDel = true;
                fansEntity.modifyDate = DateTime.Now;

                int rows = fansBLL.ActionDal.ActionDBAccess.Updateable(fansEntity).ExecuteCommand();

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
        /// 判断是否关注
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="userId">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Attention([FromForm] string token, [FromForm] int userId)
        {

            DataResult dr = new DataResult();
            try
            {
                if (userId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }
                UserEntity userEntity = this.GetUserByToken(token);
                FansBLL fansBLL = new FansBLL();

                FansEntity fansEntity = fansBLL.GetByUserIdAndAttentionId(userEntity.userId, userId);

                if (fansEntity != null)
                {
                    dr.data = true;
                }
                else
                {
                    dr.data = false;
                }

                dr.code = "200";

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);
        }


        /// <summary>
        /// 查看关注列表
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="userId">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AttentionList([FromForm] string token, [FromForm] int userId)
        {

            DataResult dr = new DataResult();
            try
            {
                if ( userId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                FansBLL fansBLL = new FansBLL();
   
                List<FansUserResult> fansUserResults = fansBLL.AttentionList(userId);

                if ( fansUserResults.Count > 0)
                {
                    UserEntity userEntity = this.GetUserByToken(token);
                    List<FansUserResult> fansUsers = fansBLL.AttentionList(userEntity.userId);
                    int[] vs = fansUserResults.Select(it => it.userId).ToArray().Intersect(fansUsers.Select(it => it.userId).ToArray()).ToArray();
                    fansUserResults.ForEach(it =>
                    {
                        if (vs.Where(itt => itt == it.userId).FirstOrDefault() > 1000)
                        {
                            it.attention = true;
                        }
                    });
                }

                dr.code = "200";
                dr.data = fansUserResults;
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);
        }

        /// <summary>
        /// 查看粉丝列表
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="userId">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FansList( [FromForm] string token, [FromForm] int userId)
        {

            DataResult dr = new DataResult();
            try
            {
                UserEntity userEntity = this.GetUserByToken(token);

                FansBLL fansBLL = new FansBLL();

                List<FansUserResult> fansUserResults = fansBLL.fansList( userId);

                if (fansUserResults.Count > 0)
                {
                    List<FansUserResult> users = fansBLL.AttentionList(userEntity.userId);

                    int[] vs = fansUserResults.Select(it => it.userId).ToArray().Intersect(users.Select(it => it.userId).ToArray()).ToArray();
                    fansUserResults.ForEach(it =>
                    {
                        if (vs.Where(itt => itt == it.userId).FirstOrDefault() > 1000)
                        {
                            it.attention = true;
                        }

                    });
                }

                

                dr.code = "200";
                dr.data = fansUserResults;
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