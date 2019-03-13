using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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

        /// <summary>
        /// 微信支付请求客户端(用于处理请求与其响应)
        /// </summary>
        private readonly IWeChatPayClient _client;

        /// <summary>
        /// 
        /// </summary>
        public CourseOrderController(IWeChatPayClient client)
        {
            courseOrderBLL = new CourseOrderBLL();
            _client = client;
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


                var request = new WeChatPayUnifiedOrderRequest
                {
                    Body = WeChat.PayEntity.body,
                    OutTradeNo = courseOrder.orderNo,
                    TotalFee = Convert.ToInt32((courseOrder.orderTotal * 100)),
                    SpbillCreateIp = WeChat.PayEntity.ip,
                    NotifyUrl = WeChat.PayEntity.notifyUrl,
                    TradeType = "APP",
                    Attach = WeChat.PayEntity.attach
                };
                var response = await _client.ExecuteAsync(request);

                if (response.ReturnCode == "SUCCESS" && response.ResultCode == "SUCCESS")
                {
                    var req = new WeChatPayAppCallPaymentRequest
                    {
                        PrepayId = response.PrepayId
                    };
                    var parameter = _client.ExecuteAsync(req);
                    // 将参数(parameter)给 ios/android端 让他调起微信APP(https://pay.weixin.qq.com/wiki/doc/api/app/app.php?chapter=8_5)
                    coursePayResult.payResult = JsonConvert.SerializeObject(parameter);
                }
                else
                {
                    coursePayResult.payResult = "获取预支付id失败";
                }

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