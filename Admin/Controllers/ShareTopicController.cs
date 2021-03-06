﻿using System;
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
    public class ShareTopicController : BaseController
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

            ShareTopicBLL shareTopicBLL = new ShareTopicBLL();
            IPagedList<ShareTopicEntity> shareTopicEntities = shareTopicBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(shareTopicEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            ShareTopicBLL shareTopicBLL = new ShareTopicBLL();
            ShareTopicEntity shareTopicEntity = shareTopicBLL.GetById(id);
            return View(shareTopicEntity);
        }
    }
}