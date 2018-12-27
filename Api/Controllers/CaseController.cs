using System;
using System.Collections.Generic;
using System.Linq;
using Api.Base;
using Api.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CaseController : BaseController
    {
        /// <summary>
        /// 阅读、点赞、积分记录的type值
        /// </summary>
        private readonly int caseType = 2;


        /// <summary>
        /// 创建案例
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="title">标题*</param>
        /// <param name="coverImage">封面照片*</param>
        /// <param name="describe">案例描述*</param>
        /// <param name="tips">小提示</param>
        /// <param name="caseStep">案例步骤:[{"img":"adasd", "contents":"asdsad"}, {"img":"asdasdadas", "contents":"asdasdsa"}]</param>
        /// <param name="caseTag">案例标签（id数组:[10001,10003]）</param>
        /// <returns>ad</returns>
        [HttpPost]
        public JsonResult Create([FromForm] string token, [FromForm] string title, [FromForm] string coverImage, 
            [FromForm] string describe, [FromForm] string tips, [FromForm] string caseStep, [FromForm] string caseTag)
        {
            DataResult dr = new DataResult();
            try
            {

                if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(describe) || string.IsNullOrWhiteSpace(coverImage))
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);

                CaseBLL caseBLL = new CaseBLL();
                CaseEntity caseEntity = caseBLL.Create( userEntity.userId, title, coverImage, describe, tips);

                string msg = string.Empty;

                if ( caseEntity != null)
                {
                    msg += "创建案例成功";

                    if (string.IsNullOrWhiteSpace(caseStep))
                    {
                        List<CaseStepEntity> caseStepEntities = JsonConvert.DeserializeObject<List<CaseStepEntity>>(caseStep);

                        caseStepEntities = caseStepEntities.Where(it => string.IsNullOrWhiteSpace(it.img) && string.IsNullOrWhiteSpace(it.contents)).ToList();

                        caseStepEntities.ForEach(it =>
                        {
                            it.caseId = caseEntity.caseId;
                        });
                        if (caseStepEntities.Count > 0)
                        {
                            int rows = caseBLL.ActionDal.ActionDBAccess.Insertable(caseStepEntities).ExecuteCommand();
                            if ( rows > 0)
                            {
                                msg += ",步骤添加成功";
                            }

                        }

                    }

                    if (string.IsNullOrWhiteSpace(caseTag))
                    {
                        int[] caseTags = JsonConvert.DeserializeObject<int[]>(caseTag);

                        if (caseTags.Length > 0)
                        {
                            List<CaseTagCorrelationEntity> caseTagCorrelationEntities = new List<CaseTagCorrelationEntity>();
                            for (int i = 0; i < caseTags.Length; i++)
                            {

                                caseTagCorrelationEntities.Add(new CaseTagCorrelationEntity
                                {
                                    caseId = caseEntity.caseId,
                                    caseTagId = caseTags[i],
                                    createDate =DateTime.Now,
                                    isDel = false,
                                    modifyDate = DateTime.Now
                                });

                                int rows = caseBLL.ActionDal.ActionDBAccess.Insertable(caseTagCorrelationEntities).ExecuteCommand();

                                if (rows > 0)
                                {
                                    msg += ",标签添加成功";
                                }

                            }


                        }

                    }

                }

                dr.code = "200";
                dr.msg = msg;
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);

        }

        /// <summary>
        /// 删除案例
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="caseId">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Delete([FromForm] string token, [FromForm] int caseId)
        {

            DataResult dr = new DataResult();
            try
            {

                if (caseId < 10000)
                {
                    dr.code = "201";
                    dr.msg = "参数错误";
                    return Json(dr);
                }

                CaseBLL caseBLL = new CaseBLL();
                CaseEntity caseEntity = caseBLL.GetById(caseId);

                if (caseEntity == null)
                {
                    dr.code = "201";
                    dr.msg = "不存在该说说";
                    return Json(dr);
                }

                UserEntity userEntity = this.GetUserByToken(token);
                if (caseEntity.userId != userEntity.userId)
                {
                    dr.code = "201";
                    dr.msg = "不是该用户的说说";
                    return Json(dr);
                }

                caseEntity.isDel = true;
                caseEntity.modifyDate = DateTime.Now;

                int rows = caseBLL.ActionDal.ActionDBAccess.Updateable(caseEntity).ExecuteCommand();

                //增加阅读记录
                ReadBLL readBLL = new ReadBLL();
                readBLL.Create(userEntity.userId, caseType, caseId);

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
        /// 获取案例列表
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="userId"></param>
        /// <param name="caseTagId"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult PageList([FromForm] string token, [FromForm] int userId = -1, [FromForm] int caseTagId = -1, [FromForm] int pageNumber = 1, [FromForm] int pageSize = 10)
        {

            DataResult dr = new DataResult();
            try
            {
                CaseBLL caseBLL = new CaseBLL();

                int totalItemCount = caseBLL.Count(caseTagId, userId: userId);
                List<CaseEntity> caseEntities = caseBLL.List(caseTagId, userId: userId, pageNumber:pageNumber, pageSize:pageSize, totalCount:totalItemCount);

                UserEntity userEntity = this.GetUserByToken(token);
                caseEntities = CaseListEndorseCountByList(caseEntities, userEntity.userId);

                caseEntities = CaseListCommentCountByList(caseEntities);

                PageData pageData = new PageData(caseEntities, pageNumber, pageSize, totalItemCount);

                dr.code = "200";
                dr.data = pageData;
            }
            catch (Exception ex)
            {
                dr.code = "999";
                dr.msg = ex.Message;
            }

            return Json(dr);

        }

        /// <summary>
        /// 根据案例ID获取详细内容
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="caseId">*</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult GetById([FromForm] string token, [FromForm] int caseId)
        {
            DataResult dr = new DataResult();
            try
            {
                CaseBLL caseBLL = new CaseBLL();
                CaseEntity caseEntity = caseBLL.GetById(caseId);
                UserEntity userEntity = this.GetUserByToken(token);
                CommentBLL commentBLL = new CommentBLL();
                caseEntity.commentCount = commentBLL.ListByTypeAndObjId(caseType, caseEntity.caseId).Count();

                EndorseBLL endorseBLL = new EndorseBLL();
                List<EndorseEntity> endorseEntities = endorseBLL.ListByTypeAndObjId(caseType, caseEntity.caseId);

                caseEntity.endorseCount = endorseEntities.Count();
                if (endorseEntities.ToList().Exists(it => it.userId == userEntity.userId))
                {
                    caseEntity.isEndorse = true;
                }

                CaseStepBLL caseStepBLL = new CaseStepBLL();
                caseEntity.caseStepEntities = caseStepBLL.ListByCaseId(caseEntity.caseId);

                CaseTagCorrelationBLL caseTagCorrelationBLL = new CaseTagCorrelationBLL();
                caseEntity.caseTagEntities = caseTagCorrelationBLL.CaseTagListByCaseId(caseEntity.caseId);

                dr.code = "200";
                dr.data = caseEntity;
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