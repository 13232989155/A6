using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Essensoft.AspNetCore.Payment.WeChatPay;
using Essensoft.AspNetCore.Payment.WeChatPay.Notify;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class WeChatController : BaseController
    {
        private readonly IWeChatPayNotifyClient _client;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public WeChatController(IWeChatPayNotifyClient client)
        {
            _client = client;
        }


        /// <summary>
        /// 微信支付结果通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public async Task<IActionResult> CoursePayNotify()
        {
            try
            {
                var notify = await _client.ExecuteAsync<WeChatPayUnifiedOrderNotify>(Request);
                if (notify.ReturnCode == "SUCCESS")
                {
                    if (notify.ResultCode == "SUCCESS")
                    {
                        Console.WriteLine("OutTradeNo: " + notify.OutTradeNo);

                        //校验订单信息
                        string payNO = notify.TransactionId;//微信订单号
                        string orderNO = notify.OutTradeNo; //商户订单号
                        string orderTotal = notify.TotalFee;//订单金额
                        string payDate = notify.TimeEnd;//支付完成时间	

                        CourseOrderBLL courseOrderBLL = new CourseOrderBLL();
                        CourseOrderEntity courseOrderEntity = courseOrderBLL.GetByOrderNO(orderNO);

                        //校验订单是否存在
                        if (courseOrderEntity == null)
                        {
                            return NoContent();
                        }

                        if (courseOrderEntity.orderTotal * 100 == Convert.ToDecimal(orderTotal))
                        {
                            //2.更新订单的相关状态
                            if (courseOrderEntity.state == 1)
                            {
                                courseOrderEntity.payNo = payNO;
                                courseOrderEntity.payDate = DateTime.ParseExact(payDate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                                courseOrderEntity.state = 2;
                                courseOrderEntity.modifyDate = DateTime.Now;
                                courseOrderEntity.realTotal = Convert.ToDecimal(orderTotal) / 100;
                                courseOrderEntity.payChannel = "微信支付-APP";
                                int rows = courseOrderBLL.ActionDal.ActionDBAccess.Updateable(courseOrderEntity).ExecuteCommand();

                                //3.返回一个xml格式的结果给微信服务器
                                if (rows > 0)
                                {
                                    return WeChatPayNotifyResult.Success;
                                }
                                else
                                {
                                    return NoContent();
                                }
                            }
                            else
                            {
                                return NoContent();
                            }
                        }
                        else
                        {
                            return NoContent();
                        }
                    }
                }
                return NoContent();
            }
            catch
            {
                return NoContent();
            }
        }
    }
}