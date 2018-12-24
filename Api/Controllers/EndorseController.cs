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
    public class EndorseController : BaseController
    {

        /// <summary>
        /// 点赞说说
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="objId">主键ID*</param>
        /// <param name="type">类型*:1 说说；2 案例</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create([FromForm] string token, [FromForm] int objId, [FromForm] int type)
        {
            DataResult dr = new DataResult();
            try
            {

                if (objId < 10000 || type < 1)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                EndorseBLL endorseBLL = new EndorseBLL();
                int rows = endorseBLL.Create(userEntity.userId, type, objId);
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
        /// 取消点赞
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="objId">主键ID*</param>
        /// <param name="type">类型*:1 说说；2 案例</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete([FromForm] string token, [FromForm] int objId, [FromForm] int type)
        {
            DataResult dr = new DataResult();
            try
            {

                if (objId < 10000 || type < 1)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);


                EndorseBLL endorseBLL = new EndorseBLL();
                EndorseEntity endorseEntity = endorseBLL.GetUserIdAndTypeAndObjId(userEntity.userId, type, objId);

                endorseEntity.isDel = true;
                endorseEntity.modifyDate = DateTime.Now;

                int rows = endorseBLL.ActionDal.ActionDBAccess.Updateable(endorseEntity).ExecuteCommand();
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



    }
}