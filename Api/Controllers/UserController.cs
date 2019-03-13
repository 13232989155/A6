using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using WeChat;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : BaseController
    {

        private UserBLL userBLL = null;

        /// <summary>
        /// 用户
        /// </summary>
        public UserController()
        {
            this.userBLL = new UserBLL();
        }

        /// <summary>
        /// 微信登录
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [SkipCheckLogin]
        [HttpPost]
        public JsonResult WxLogin([FromForm] string code)
        {
            DataResult dr = new DataResult();
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                //UserEntity userEntity = userBLL.GetById(10007);
                //userEntity.account = code;
                //userBLL.ActionDal.ActionDBAccess.Updateable(userEntity).ExecuteCommand();

                AccessTokenEntity accessTokenEntity = WeChat.LoginHelper.GetAccessToken(code);

                if ( !string.IsNullOrWhiteSpace(accessTokenEntity.errcode))
                {
                    dr.code = "201";
                    dr.msg = "获取AccessToken失败";
                    return Json(dr);
                }

                AccessTokenEntity accessToken = WeChat.LoginHelper.GetRefreshToken(accessTokenEntity.refresh_token);

                if ( !string.IsNullOrWhiteSpace(accessToken.errcode))
                {
                    dr.code = "201";
                    dr.msg = "获取RefreshToken失败";
                    return Json(dr);
                }

                AccessTokenEntity tokenEntity = WeChat.LoginHelper.SetExpiresIn(accessToken);

                if (string.IsNullOrWhiteSpace(tokenEntity.access_token) || string.IsNullOrWhiteSpace(tokenEntity.openid))
                {
                    dr.code = "201";
                    dr.msg = "续期失败";
                    return Json(dr);
                }

                WeChat.WxUserEntity wxUserEntity = WeChat.LoginHelper.GetWxUser(tokenEntity.access_token, tokenEntity.openid);

                if ( string.IsNullOrWhiteSpace(wxUserEntity.openid) || !string.IsNullOrWhiteSpace(wxUserEntity.errcode))
                {
                    dr.code = "201";
                    dr.msg = "获取用户信息失败";
                    return Json(dr);
                }

                WxUserBLL wxUserBLL = new WxUserBLL();

                Entity.WxUserEntity wxUser = wxUserBLL.GetByOpenId(wxUserEntity.openid);

                if (wxUser == null)
                {
                    int rows = CreateWxUser(wxUserEntity);
                    if (rows > 0)
                    {
                        dr.code = "201";
                        dr.msg = "创建用户失败";
                        return Json(dr);

                    }

                }

                Entity.WxUserEntity wx = wxUserBLL.GetByOpenId(wxUserEntity.openid);

                UserEntity user = userBLL.GetById(wx.userId);

                UserTokenBLL userTokenBLL = new UserTokenBLL();
                UserTokenEntity userTokenEntity = userTokenBLL.GetByUserId(user.userId);
                UserTokenEntity userToken = new UserTokenEntity();

                if (userTokenEntity == null)
                {
                    userToken = userTokenBLL.Create(user.userId);
                }
                else
                {
                    userToken = userTokenBLL.Update(userTokenEntity);
                }

                LoginResult loginResult = new LoginResult();
                loginResult.token = userToken.token;
                loginResult.userEntity = user;

                dr.code = "200";
                dr.data = loginResult;


            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);
        }

        /// <summary>
        /// 创建微信用户
        /// </summary>
        /// <param name="wxUserEntity"></param>
        /// <returns></returns>
        private int CreateWxUser(WeChat.WxUserEntity wxUserEntity)
        {
            UserEntity userEntity = new UserEntity()
            {
                account = "",
                address = "",
                birthday = Helper.ConvertHelper.DEFAULT_DATE,
                createDate = DateTime.Now,
                email = "",
                forbidden = false,
                gender = Convert.ToInt32(wxUserEntity.sex),
                grade = 1,
                integral = 1000,
                modifyDate = DateTime.Now,
                name = wxUserEntity.nickname,
                password = "",
                phone = "",
                portrait = wxUserEntity.headimgurl,
                remark = "",
                signature = "",
                state = -1,
                type = -1
            };

            UserEntity user = userBLL.ActionDal.ActionDBAccess.Insertable(userEntity).ExecuteReturnEntity();

            Entity.WxUserEntity wxUser = new Entity.WxUserEntity()
            {
                avatarUrl = wxUserEntity.headimgurl,
                city = wxUserEntity.city,
                country = wxUserEntity.country,
                createDate = DateTime.Now,
                gender = wxUserEntity.sex,
                modifyDate = DateTime.Now,
                nickName = wxUserEntity.nickname,
                openId = wxUserEntity.openid,
                province = wxUserEntity.province,
                unionId = wxUserEntity.unionId ?? "",
                userId = user.userId
            };
            int rows = userBLL.ActionDal.ActionDBAccess.Insertable(wxUser).ExecuteCommand();

            return rows;
        }

        /// <summary>
        /// 手机号码登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        [SkipCheckLogin]
        [HttpPost]
        public JsonResult PhoneLogin([FromForm] string phone, [FromForm] string code)
        {

            DataResult dr = new DataResult();
            try
            {
                DataResult dataResult = ExaminePhoneAndCode(phone, code);

                if (dataResult.code != "200")
                {
                    return Json(dataResult);
                }

                UserEntity userEntity = userBLL.GetByPhone(phone);

                if (userEntity == null)
                {
                    int rows = userBLL.CreateToPhone(phone);
                }

                PhoneCodeBLL phoneCodeBLL = new PhoneCodeBLL();
                phoneCodeBLL.Delete(phone);
                UserEntity user = userBLL.GetByPhone(phone);

                UserTokenBLL userTokenBLL = new UserTokenBLL();
                UserTokenEntity userTokenEntity = userTokenBLL.GetByUserId(user.userId);
                UserTokenEntity userToken = new UserTokenEntity();

                if (userTokenEntity == null)
                {
                    userToken = userTokenBLL.Create(user.userId);
                }
                else
                {
                    userToken = userTokenBLL.Update(userTokenEntity);
                }

                LoginResult loginResult = new LoginResult();
                loginResult.token = userToken.token;
                loginResult.userEntity = user;

                dr.code = "200";
                dr.data = loginResult;

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);

        }

        /// <summary>
        /// 手机号码和密码登录
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [SkipCheckLogin]
        [HttpPost]
        public JsonResult PhonePasswordLogin([FromForm] string phone, [FromForm] string password)
        {

            DataResult dr = new DataResult();
            try
            {
                if (string.IsNullOrWhiteSpace(phone) || phone.Length != 11)
                {
                    dr.code = "201";
                    dr.msg = "手机号码错误";
                    return Json(dr);
                }
                UserEntity user = userBLL.GetByPhoneAndPassword(phone, Helper.DataEncrypt.DataMd5( password));

                if (user == null)
                {
                    dr.code = "201";
                    dr.msg = "手机号码或密码错误错误";
                    return Json(dr);
                }

                UserTokenBLL userTokenBLL = new UserTokenBLL();
                UserTokenEntity userTokenEntity = userTokenBLL.GetByUserId(user.userId);
                UserTokenEntity userToken = new UserTokenEntity();

                if (userTokenEntity == null)
                {
                    userToken = userTokenBLL.Create(user.userId);
                }
                else
                {
                    userToken = userTokenBLL.Update(userTokenEntity);
                }

                LoginResult loginResult = new LoginResult();
                loginResult.token = userToken.token;
                loginResult.userEntity = user;

                dr.code = "200";
                dr.data = loginResult;

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);

        }

        /// <summary>
        /// 检查手机号码
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private DataResult ExaminePhoneAndCode(string phone, string code)
        {
            DataResult dr = new DataResult();
            try
            {

                if (string.IsNullOrWhiteSpace(phone) || phone.Length != 11)// || Regex.IsMatch(phone, Helper.RegexHelper.PATTERN_PHONE)
                {
                    dr.code = "201";
                    dr.msg = "手机号码错误";
                    return dr;
                }

                if (string.IsNullOrWhiteSpace(code) || code.Length != 6)
                {
                    dr.code = "201";
                    dr.msg = "验证码错误";
                    return dr;
                }

                PhoneCodeBLL phoneCodeBLL = new PhoneCodeBLL();
                PhoneCodeEntity phoneCodeEntity = phoneCodeBLL.GetByPhoneAndCode(phone, code);

                if (phoneCodeEntity == null)
                {
                    dr.code = "201";
                    dr.msg = "验证码错误";
                    return dr;
                }

                TimeSpan ts = DateTime.Now.Subtract(phoneCodeEntity.createDate).Duration();
                double dateDiff = ts.TotalMinutes;

                if (dateDiff > 15)
                {
                    phoneCodeBLL.Delete(phone);
                    dr.code = "201";
                    dr.msg = "验证码过期";
                    return dr;
                }

                dr.code = "200";

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return dr;
        }

        /// <summary>
        /// 获取个人信息
        /// </summary>
        /// <param name="token">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Get([FromForm] string token)
        {
            DataResult dr = new DataResult();
            try
            {

                dr.code = "200";
                dr.data = this.GetUserByToken(token);

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);
        }

        /// <summary>
        /// 设置自己信息
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="userEntity">用户信息*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Set([FromForm] string token, [FromForm] UserEntity userEntity)
        {
            DataResult dr = new DataResult();
            try
            {

                UserEntity user = this.GetUserByToken(token);
                user.address = userEntity.address ?? "";
                user.birthday = userEntity.birthday == null ? Helper.ConvertHelper.DEFAULT_DATE : userEntity.birthday;
                user.email = userEntity.email ?? "";
                user.gender = userEntity.gender;
                user.modifyDate = DateTime.Now;
                user.name = userEntity.name == null ? user.userId.ToString() : userEntity.name;
                user.phone = userEntity.phone ?? "";
                user.portrait = userEntity.portrait;
                user.signature = userEntity.signature ?? "";
                int rows = userBLL.ActionDal.ActionDBAccess.Updateable(user).ExecuteCommand();

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
        /// 获取他人信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userId">*</param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult GetById([FromForm] string token, [FromForm] int userId)
        {
            DataResult dr = new DataResult();
            try
            {
                UserEntity userEntity = userBLL.GetById(userId);
                FansUserResult fansUserResult = new FansUserResult();
                if (userEntity != null)
                {
                    fansUserResult.userId = userEntity.userId;
                    fansUserResult.attention = false;
                    fansUserResult.birthday = userEntity.birthday;
                    fansUserResult.gender = userEntity.gender;
                    fansUserResult.name = userEntity.name;
                    fansUserResult.portrait = userEntity.portrait;
                    fansUserResult.signature = userEntity.signature;
                }
                dr.code = "200";
                dr.data = fansUserResult;

            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);
        }


        /// <summary>
        /// 生成实体文件
        /// </summary>
        [HttpGet]
        [SkipCheckLogin]
        public void DbFirst()
        {

            //userBLL.ActionDal.ActionDBAccess.DbFirst.Where("t_case").CreateClassFile("c:\\A6");
            //userBLL.ActionDal.ActionDBAccess.DbFirst.Where("t_case_step").CreateClassFile("c:\\A6");
            //userBLL.ActionDal.ActionDBAccess.DbFirst.Where("t_case_tag").CreateClassFile("c:\\A6");
            //userBLL.ActionDal.ActionDBAccess.DbFirst.Where("t_case_tag_correlation").CreateClassFile("c:\\A6");
        }


    }
}