﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ShareTopicController : BaseController
    {

        /// <summary>
        /// 获取说说话题列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public JsonResult List()
        {

            DataResult dr = new DataResult();
            try
            {
                ShareTopicBLL shareTopicBLL = new ShareTopicBLL();

                List<ShareTopicEntity> shareTopicEntities = shareTopicBLL.UnderwayList();

                dr.code = "200";
                dr.data = shareTopicEntities;
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