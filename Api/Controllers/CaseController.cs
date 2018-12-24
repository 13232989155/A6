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



    }
}