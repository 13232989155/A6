using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Base;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Admin.Controllers
{
    public class UserController : BaseController
    {
        /// <summary>
        /// 列表页面
        /// </summary>
        /// <param name="searchString"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult List(string searchString, int? page)
        {
            ViewBag.searchString = string.IsNullOrWhiteSpace(searchString) ? "" : searchString;
            int pageNumber = (page ?? 1);
            int pageSize = 15;
            UserBLL userBLL = new UserBLL();
            IPagedList<UserEntity> userEntities = userBLL.AdminPageList(pageNumber, pageSize, searchString);
            return View(userEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IActionResult Detail(int userId)
        {
            UserBLL userBLL = new UserBLL();
            UserEntity userEntity = userBLL.GetById(userId);

            WxUserBLL wxUserBLL = new WxUserBLL();
            userEntity.wxUserEntity = wxUserBLL.GetByUserId(userEntity.userId);

            return View(userEntity);
        }

    }
}