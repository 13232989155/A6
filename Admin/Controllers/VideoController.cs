using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Base;
using Admin.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Admin.Controllers
{
    public class VideoController : BaseController
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

            VideoBLL videoBLL = new VideoBLL();
            IPagedList<VideoEntity> videoEntities = videoBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(videoEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int id)
        {
            VideoBLL videoBLL = new VideoBLL();
            VideoEntity videoEntity = videoBLL.GetById(id);
            return View(videoEntity);
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="adminEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create([Bind("remark, url")] VideoEntity videoEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(videoEntity.url))
                {
                    dataResult.code = "201";
                    dataResult.msg = "视频不能为空";
                    return dataResult;
                }

                VideoBLL videoBLL = new VideoBLL();
                VideoEntity video = new VideoEntity()
                {
                    createDate = DateTime.Now,
                    modifyDate = DateTime.Now,
                    remark = videoEntity.remark ?? "",
                    url = videoEntity.url
                };
                        
                int rows = videoBLL.ActionDal.ActionDBAccess.Insertable(video).ExecuteCommand();

                if (rows > 0)
                {
                    dataResult.code = "200";
                    dataResult.msg = "成功";
                }
                else
                {
                    dataResult.code = "201";
                    dataResult.msg = "失败";
                }

            }
            catch (Exception e)
            {
                dataResult.code = "202";
                dataResult.msg = e.Message;
                return dataResult;
            }

            return dataResult;
        }
    }
}