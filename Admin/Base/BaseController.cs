using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using Newtonsoft.Json;

namespace Admin.Base
{
    public class BaseController : Controller
    {
        /// <summary>
        /// 判断是否登录
        /// </summary>
        /// <returns></returns>
        protected bool IsLogin()
        {
            bool isLogin = false;

            if (ThisAdmin() != null)
            {
                isLogin = true;
            }

            return isLogin;
        }

        /// <summary>
        /// 返回当前登录用户
        /// </summary>
        /// <returns></returns>
        public AdminEntity ThisAdmin()
        {

            AdminEntity adminEntity = HttpContext.Session.Get<AdminEntity>("admin");

            if (adminEntity == null)
            {
                HttpContext.Session.Clear();
                Response.Redirect("/Login/Login");
            }

            return adminEntity;
        }

    }
}