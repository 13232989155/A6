using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using PaySharp.Core;
using PaySharp.Wechatpay.Response;

namespace Admin.Controllers
{
    public class WeChatController : Controller
    {

        private readonly IGateways _gateways;

        public WeChatController(IGateways gateways)
        {
            _gateways = gateways;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private bool Notify_PaySucceed(object sender, PaySucceedEventArgs e)
        {
            // 支付成功时时的处理代码
            /* 建议添加以下校验。
             * 1、需要验证该通知数据中的OutTradeNo是否为商户系统中创建的订单号，
             * 2、判断Amount是否确实为该订单的实际金额（即商户订单创建时的金额），
             */
            if (e.GatewayType == typeof(PaySharp.Wechatpay.WechatpayGateway))
            {
                var alipayNotifyResponse = (NotifyResponse)e.NotifyResponse;

                //校验订单信息
                string payNO = alipayNotifyResponse.TradeNo;//微信订单号
                string orderNO = alipayNotifyResponse.OutTradeNo; //商户订单号
                string orderTotal = alipayNotifyResponse.TotalAmount.ToString();//订单金额
                string payDate = alipayNotifyResponse.TimeEnd;//支付完成时间
                
                CourseOrderBLL courseOrderBLL = new CourseOrderBLL();
                CourseOrderEntity courseOrderEntity = courseOrderBLL.GetByOrderNO(orderNO);

                //校验订单是否存在
                if (courseOrderEntity == null)
                {
                    return false;
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
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            //处理成功返回true
            return false;
        }

        /// <summary>
        /// 微信支付结果通知
        /// </summary>
        /// <returns></returns>
        public async Task CoursePayNotify()
        {
            try
            {

                // 订阅支付通知事件
                Notify notify = new Notify(_gateways);
                notify.PaySucceed += Notify_PaySucceed;

                // 接收并处理支付通知
                await notify.ReceivedAsync();

            }
            catch (Exception ex)
            {
                CourseOrderBLL courseOrderBLL = new CourseOrderBLL();
                UserEntity userEntity = courseOrderBLL.ActionDal.ActionDBAccess.Queryable<UserEntity>()
                                            .Where(it => it.userId == 10009).First();
                userEntity.account = ex.Message;
                userEntity.modifyDate = DateTime.Now;
                courseOrderBLL.ActionDal.ActionDBAccess.Insertable(userEntity).ExecuteCommand();
            }
        }
    }
}