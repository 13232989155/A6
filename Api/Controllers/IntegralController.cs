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
    public class IntegralController : BaseController
    {
        /// <summary>
        /// 打赏
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="objId">对象主键ID*</param>
        /// <param name="type">对象类型*：1 说说； 2 案列；</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Create([FromForm] string token, [FromForm] int objId, [FromForm] int type)
        {
            DataResult dr = new DataResult();
            try
            {

                if (objId < 10000  || type < 1)
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

                if (type == 1)
                {
                    ShareBLL shareBLL = new ShareBLL();

                    ShareEntity shareEntity = shareBLL.GetById(objId);
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
                            objId = objId,
                            type = type,
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
                            objId = objId,
                            type = type,
                            userId = user.userId
                        };
                        var rows4 = shareBLL.ActionDal.ActionDBAccess.Insertable(integralDetail).ExecuteCommand();

                        shareEntity.integral = shareEntity.integral + 1;
                        var rows5 = shareBLL.ActionDal.ActionDBAccess.Updateable(shareEntity).ExecuteCommand();
                    });
                    //增加阅读记录
                    ReadBLL readBLL = new ReadBLL();
                    readBLL.Create(userEntity.userId, type, objId);
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
                else
                {

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