using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Base;
using Admin.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Admin.Controllers
{
    public class CourseSectionController : BaseController
    {
        private CourseSectionBLL courseSectionBLL = null;

        public CourseSectionController()
        {
            courseSectionBLL = new CourseSectionBLL();
        }

        /// <summary>
        /// 视频章节列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> List(int id)
        {
            DataResult dataResult = new DataResult();
            try
            {
                List<CourseSectionEntity> courseSectionEntities = courseSectionBLL.ListByCourseId(id);
                dataResult.data = courseSectionEntities;
                dataResult.code = "200";
            }
            catch (Exception e)
            {
                dataResult.code = "999";
                dataResult.msg = e.Message;
                return dataResult;
            }
            return dataResult;
        }

        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="courseSectionEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create([Bind("courseId, name, videoUrl, courseSectionId")] CourseSectionEntity courseSectionEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(courseSectionEntity.videoUrl))
                {
                    dataResult.code = "201";
                    dataResult.msg = "视频不能为空";
                    return dataResult;
                }

                int rows = 0;
                if (courseSectionEntity.courseSectionId > 1000)
                {
                    CourseSectionEntity courseSection = courseSectionBLL.GetById(courseSectionEntity.courseSectionId);
                    courseSection.name = courseSectionEntity.name ?? "";
                    courseSection.videoUrl = courseSectionEntity.videoUrl;

                    rows = courseSectionBLL.ActionDal.ActionDBAccess.Updateable(courseSection).ExecuteCommand();
                }
                else
                {
                    CourseSectionEntity courseSection = new CourseSectionEntity()
                    {
                        courseId = courseSectionEntity.courseId,
                        name = courseSectionEntity.name ?? "",
                        videoUrl = courseSectionEntity.videoUrl
                    };

                    rows = courseSectionBLL.ActionDal.ActionDBAccess.Insertable(courseSection).ExecuteCommand();
                }
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
                dataResult.code = "999";
                dataResult.msg = e.Message;
                return dataResult;
            }

            return dataResult;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Delete( int id)
        {
            DataResult dataResult = new DataResult();

            try
            {

                CourseSectionEntity courseSectionEntity = courseSectionBLL.GetById(id);

                int rows = courseSectionBLL.ActionDal.ActionDBAccess.Deleteable(courseSectionEntity).ExecuteCommand();

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
                dataResult.code = "999";
                dataResult.msg = e.Message;
                return dataResult;
            }

            return dataResult;
        }


    }
}