using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Admin.Models;
using Admin.Base;

namespace Admin.Controllers
{
    public class LoginController : BaseController
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string account, string password)
        {

            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.msg = "账号或密码不能为空！";
                return View();
            }
            AdminBLL adminBLL = new AdminBLL();
            AdminEntity adminEntity = adminBLL.GetAccountAndPassword(account, Helper.DataEncrypt.DataMd5(password));

            if (adminEntity == null)
            {
                ViewBag.msg = "账号或密码错误！";
                return View();
            }

            if (adminEntity.forbidden)
            {
                ViewBag.msg = "账号已被禁用！";
                return View();
            }

            HttpContext.Session.Set("admin", adminEntity);

            return RedirectToAction(controllerName: "Home", actionName: "Index");
        }

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


        /// <summary>
        /// 返回当前用户
        /// </summary>
        /// <returns></returns>
        public AdminEntity GetThisAdmin()
        {
            return this.ThisAdmin();
        }


    }
}