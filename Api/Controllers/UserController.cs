﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using X.PagedList;

namespace Api.Controllers
{
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
        /// 用户注册
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns>msg</returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult Register([FromForm] string account, [FromForm] string password )
        {
            DataResult dr = new DataResult();
            try
            {

                if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
                {
                    dr.msg = "账号或密码为空";
                    dr.code = "201";
                    return Json(dr);
                }

                UserEntity user = userBLL.GetByAccount(account);
                if (user != null)
                {
                    dr.msg = "已存在该账号";
                    dr.code = "201";
                    return Json(dr);
                }

                int rows = userBLL.Register(account, password);
                if (rows > 0)
                {
                    dr.msg = "注册成功";
                    dr.code = "200";
                }
                else
                {
                    dr.msg = "注册失败";
                    dr.code = "201";
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
        /// 用户登录
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        [SkipCheckLogin]
        [HttpPost]
        public JsonResult Login([FromForm] string account, [FromForm] string password)
        {
            DataResult dr = new DataResult();
            try
            {
                if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
                {
                    dr.msg = "账号或密码为空";
                    dr.code = "201";
                    return Json(dr);
                }

                UserEntity user = userBLL.GetByAccountAndPassword(account, password);

                if (user == null)
                {
                    dr.msg = "账号或密码错误";
                    dr.code = "201";
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
        /// 获取个人信息
        /// </summary>
        /// <param name="token"></param>
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
        /// 设置个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="userEntity">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Set([FromForm] string token, [FromForm] UserEntity userEntity)
        {
            DataResult dr = new DataResult();
            try
            {

                UserEntity user = this.GetUserByToken(token);

                dr.code = "200";
                dr.data = user;

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
        [HttpGet]
        [SkipCheckLogin]
        public void DbFirst()
        {

            //userBLL.ActionDal.ActionDBAccess.DbFirst.Where("t_topic").CreateClassFile("c:\\A6");
            //userBLL.ActionDal.ActionDBAccess.DbFirst.Where("t_integral_detail").CreateClassFile("c:\\A6");
        }


    }
}