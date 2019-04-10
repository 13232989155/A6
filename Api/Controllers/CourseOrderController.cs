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
using Newtonsoft.Json;
using PaySharp.Core;
using PaySharp.Wechatpay;
using PaySharp.Wechatpay.Domain;
using PaySharp.Wechatpay.Request;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CourseOrderController : BaseController
    {

        /// <summary>
        /// 
        /// </summary>
        private CourseOrderBLL courseOrderBLL = null;

        private readonly IGateway _gateway;

        /// <summary>
        /// 
        /// </summary>
        public CourseOrderController(IGateways gateways)
        {
            courseOrderBLL = new CourseOrderBLL();
             _gateway = gateways.Get<WechatpayGateway>();
        }

        /// <summary>
        /// 创建订单并支付
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="courseId">*</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<JsonResult> Create([FromForm] string token, [FromForm] int courseId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (courseId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                CourseEntity courseEntity = courseOrderBLL.ActionDal.ActionDBAccess.Queryable<CourseEntity>().InSingle(courseId);

                if (courseEntity == null)
                {
                    dr.code = "201";
                    dr.msg = "不存在该课程";
                    return Json(dr);
                }

                CourseOrderEntity courseOrderEntity = new CourseOrderEntity()
                {
                    courseId = courseEntity.courseId,
                    createDate = DateTime.Now,
                    modifyDate = DateTime.Now,
                    orderNo = "WX" + userEntity.userId.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    orderTotal = courseEntity.price,
                    payChannel = "",
                    payDate = Helper.ConvertHelper.DEFAULT_DATE,
                    payNo = "",
                    realTotal = 0,
                    state = 1,
                    userId = userEntity.userId
                };

                CourseOrderEntity courseOrder = await courseOrderBLL.ActionDal.ActionDBAccess.Insertable(courseOrderEntity).ExecuteReturnEntityAsync();

                if ( courseOrder == null)
                {
                    dr.code = "201";
                    dr.msg = "创建订单失败";
                    return Json(dr);
                }

                CoursePayResult coursePayResult = new CoursePayResult();
                coursePayResult.courseOrderEntity = courseOrder;

                var request = new AppPayRequest();
                request.AddGatewayData(new AppPayModel()
                {
                    Body = "购买课程" + courseEntity.name,
                    TotalAmount = Convert.ToInt32( courseOrder.orderTotal * 100),
                    OutTradeNo = courseOrder.orderNo
                });

                var response = _gateway.Execute(request);
                coursePayResult.payResult = response.OrderInfo;

                dr.code = "200";
                dr.msg = "成功";
                dr.data = coursePayResult;

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);
        }

        /// <summary>
        /// 查询课程是否已购买
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="courseId">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Get([FromForm] string token, [FromForm] int courseId)
        {
            DataResult dr = new DataResult();
            try
            {

                if (courseId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                CourseOrderEntity courseOrderEntity = courseOrderBLL.GetByCourseAndUserId( courseId, userEntity.userId);

                dr.code = "200";
                dr.msg = "成功";
                dr.data = courseOrderEntity;

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }
            return Json(dr);
        }


        /// <summary>
        /// 查看已购买的课程
        /// </summary>
        /// <param name="token">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult ListByUserId([FromForm] string token)
        {
            DataResult dr = new DataResult();
            try
            {
                UserEntity userEntity = this.GetUserByToken(token);

                List<CourseEntity> courseEntities = courseOrderBLL.ListByUserId(userEntity.userId);

                dr.code = "200";
                dr.msg = "成功";
                dr.data = courseEntities;

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