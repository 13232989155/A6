using System;
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
using X.PagedList;

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

                IEnumerable<CaseEntity> caseEntities = caseBLL.List(caseTagId, userId: userId);
                UserEntity userEntity = this.GetUserByToken(token);

                IPagedList<CaseEntity> cases  = caseEntities.ToList().ToPagedList(pageNumber, pageSize);

                cases = ListEndorseCountByList(cases, userEntity.userId);

                cases = ListCommentCountByList(cases);

                PageData pageData = new PageData(cases);

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
        /// 获取评论数量
        /// </summary>
        /// <param name="caseEntities"></param>
        /// <returns></returns>
        private IPagedList<CaseEntity> ListCommentCountByList(IPagedList<CaseEntity> caseEntities)
        {
            if (caseEntities.Count() > 0)
            {
                int[] idInts = caseEntities.Select(it => it.caseId).ToArray();

                CommentBLL commentBLL = new CommentBLL();
                IEnumerable<CommentEntity> commentEntities = commentBLL.ListByTypeAndObjIdInts(caseType, idInts);
                if (commentEntities.Count() > 0)
                {
                    Dictionary<int, int> keyValuePairs = commentEntities
                                                        .GroupBy(it => it.objId)
                                                        .Select(it => new
                                                        {
                                                            id = it.Key,
                                                            count = it.Count()
                                                        })
                                                        .ToDictionary(it => it.id, it => it.count);

                    caseEntities = (from c in caseEntities
                                     join e in keyValuePairs on c.caseId equals e.Key into se
                                     from ses in se.DefaultIfEmpty()
                                     select new CaseEntity
                                     {
                                         caseId = c.caseId,
                                         coverImage = c.coverImage,
                                         createDate = c.createDate,
                                         describe = c.describe,
                                         integral = c.integral,
                                         isDel = c.isDel,
                                         modifyDate = c.modifyDate,
                                         name = c.name,
                                         portrait = c.portrait,
                                         state = c.state,
                                         tips = c.tips,
                                         title = c.title,
                                         userId = c.userId,
                                         commentCount = ses.Value,
                                         endorseCount = c.endorseCount,
                                         isEndorse = c.isEndorse,
                                         readCount = c.readCount
                                     })
                                     .ToPagedList();
                }
            }

            return caseEntities;
        }

        /// <summary>
        /// 获取点赞数量和判断是否点赞
        /// </summary>
        /// <param name="caseEntities"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private IPagedList<CaseEntity> ListEndorseCountByList(IPagedList<CaseEntity> caseEntities, int userId)
        {
            if (caseEntities.Count() > 0)
            {
                int[] idInts = caseEntities.Select(it => it.caseId).ToArray();

                EndorseBLL endorseBLL = new EndorseBLL();
                IEnumerable<EndorseEntity> endorseEntities = endorseBLL.ListByTypeAndObjIdInts(caseType, idInts);

                if (endorseEntities.Count() > 0)
                {

                    var userEndorseList = endorseEntities.Where(it => it.userId == userId).ToList();

                    userEndorseList.ToList().ForEach(it =>
                    {
                        caseEntities.First(itt => itt.caseId == it.objId).isEndorse = true;
                    });

                    Dictionary<int, int> keyValuePairs = endorseEntities
                                                        .GroupBy(it => it.objId)
                                                        .Select(it => new
                                                        {
                                                            id = it.Key,
                                                            count = it.Count()
                                                        })
                                                        .ToDictionary(it => it.id, it => it.count);

                    caseEntities = (from c in caseEntities
                                    join e in keyValuePairs on c.caseId equals e.Key into se
                                     from ses in se.DefaultIfEmpty()
                                    select new CaseEntity
                                    {
                                        caseId = c.caseId,
                                        coverImage = c.coverImage,
                                        createDate = c.createDate,
                                        describe = c.describe,
                                        integral = c.integral,
                                        isDel = c.isDel,
                                        modifyDate = c.modifyDate,
                                        name = c.name,
                                        portrait = c.portrait,
                                        state = c.state,
                                        tips = c.tips,
                                        title = c.title,
                                        userId = c.userId,
                                        commentCount = c.commentCount,
                                        endorseCount = ses.Value,
                                        isEndorse = c.isEndorse,
                                        readCount = c.readCount
                                    })
                                     .ToPagedList();

                }

            }

            return caseEntities;
        }


        /// <summary>
        /// 根据说说ID获取详细内容
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
                IEnumerable<EndorseEntity> endorseEntities = endorseBLL.ListByTypeAndObjId(caseType, caseEntity.caseId);

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