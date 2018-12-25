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
                    return Json(SponsorShare(userEntity, objId, type));
                }
                else
                {
                    return Json(SponsorCase(userEntity, objId, type));
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
        /// 打赏说说
        /// </summary>
        /// <param name="userEntity"></param>
        /// <param name="shareId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private DataResult SponsorShare(UserEntity userEntity, int shareId, int type)
        {
            DataResult dr = new DataResult();

            ShareBLL shareBLL = new ShareBLL();

            ShareEntity shareEntity = shareBLL.GetById(shareId);
            if (shareEntity.isDel)
            {
                dr.code = "201";
                dr.msg = "说说已被删除";
                return dr;
            }
            if (shareEntity.userId < 10000)
            {
                dr.code = "201";
                dr.msg = "无主说说";
                return dr;
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
                    objId = shareId,
                    type = type,
                    userId = user.userId
                };
                var rows4 = shareBLL.ActionDal.ActionDBAccess.Insertable(integralDetail).ExecuteCommand();

                shareEntity.integral = shareEntity.integral + 1;
                var rows5 = shareBLL.ActionDal.ActionDBAccess.Updateable(shareEntity).ExecuteCommand();
            });
            //增加阅读记录
            ReadBLL readBLL = new ReadBLL();
            readBLL.Create(userEntity.userId, type, shareId);
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

            return dr;
        }

        /// <summary>
        /// 打赏案例
        /// </summary>
        /// <param name="userEntity"></param>
        /// <param name="caseId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private DataResult SponsorCase(UserEntity userEntity, int caseId, int type)
        {
            DataResult dr = new DataResult();

            CaseBLL caseBLL = new CaseBLL();
            CaseEntity caseEntity = caseBLL.GetById(caseId);

            if (caseEntity.isDel)
            {
                dr.code = "201";
                dr.msg = "说说已被删除";
                return dr;
            }
            if (caseEntity.userId < 10000)
            {
                dr.code = "201";
                dr.msg = "无主案例";
                return dr;
            }

            var result = caseBLL.ActionDal.ActionDBAccess.Ado.UseTran(() =>
            {
                //积分减1
                userEntity.integral = userEntity.integral - 1;
                var rows1 = caseBLL.ActionDal.ActionDBAccess.Updateable(userEntity).ExecuteCommand();
                IntegralDetailEntity integralDetailEntity = new IntegralDetailEntity()
                {
                    createDate = DateTime.Now,
                    integral = -1,
                    isDel = false,
                    modifyDate = DateTime.Now,
                    objId = caseId,
                    type = type,
                    userId = userEntity.userId
                };
                var rows2 = caseBLL.ActionDal.ActionDBAccess.Insertable(integralDetailEntity).ExecuteCommand();

                //积分加1
                UserEntity user = caseBLL.ActionDal.ActionDBAccess.Queryable<UserEntity>().Where(it => it.userId == caseEntity.userId).First();
                user.integral = user.integral + 1;
                var rows3 = caseBLL.ActionDal.ActionDBAccess.Updateable(user).ExecuteCommand();
                IntegralDetailEntity integralDetail = new IntegralDetailEntity()
                {
                    createDate = DateTime.Now,
                    integral = 1,
                    isDel = false,
                    modifyDate = DateTime.Now,
                    objId = caseId,
                    type = type,
                    userId = user.userId
                };
                var rows4 = caseBLL.ActionDal.ActionDBAccess.Insertable(integralDetail).ExecuteCommand();

                caseEntity.integral = caseEntity.integral + 1;
                var rows5 = caseBLL.ActionDal.ActionDBAccess.Updateable(caseEntity).ExecuteCommand();
            });
            //增加阅读记录
            ReadBLL readBLL = new ReadBLL();
            readBLL.Create(userEntity.userId, type, caseId);
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

            return dr;
        }



    }
}