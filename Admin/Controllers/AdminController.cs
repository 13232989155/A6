using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Admin.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Admin.Controllers
{
    public class AdminController : Controller
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

            AdminBLL adminBLL = new AdminBLL();
            IPagedList<AdminEntity> adminEntities = adminBLL.AdminPageList(pageNumber, pageSize, searchString);

            return View(adminEntities);
        }

        /// <summary>
        /// 详细页面
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public IActionResult Detail(int adminId)
        {
            AdminBLL adminBLL = new AdminBLL();
            AdminEntity adminEntity = adminBLL.GetById(adminId);

            return View(adminEntity);
        }

        /// <summary>
        /// 创建页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            UserBLL userBLL = new UserBLL();
            List<UserEntity> userEntities = userBLL.List();
            return View(userEntities);  
        }

        /// <summary>
        /// 保存创建
        /// </summary>
        /// <param name="adminEntity"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Create( AdminEntity adminEntity)
        {
            DataResult dataResult = new DataResult();

            try
            {

                if (string.IsNullOrWhiteSpace(adminEntity.name))
                {
                    dataResult.code = "201";
                    dataResult.msg = "名称不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(adminEntity.account))
                {
                    dataResult.code = "201";
                    dataResult.msg = "账号不能为空";
                    return dataResult;
                }

                if (string.IsNullOrWhiteSpace(adminEntity.password))
                {
                    dataResult.code = "201";
                    dataResult.msg = "密码不能为空";
                    return dataResult;
                }

                AdminBLL adminBLL = new AdminBLL();

                List<AdminEntity> adminEntitiesByName = adminBLL.ActionDal.ActionDBAccess.Queryable<AdminEntity>()
                                                    .Where(it => it.name == adminEntity.name )
                                                    .ToList();

                if (adminEntitiesByName.Count > 0)
                {
                    dataResult.code = "201";
                    dataResult.msg = "已存在该用户名称";
                    return dataResult;
                }

                List<AdminEntity> adminEntitiesByAccount = adminBLL.ActionDal.ActionDBAccess.Queryable<AdminEntity>()
                                                   .Where(it => it.account == adminEntity.account)
                                                   .ToList();

                if (adminEntitiesByAccount.Count > 0)
                {
                    dataResult.code = "201";
                    dataResult.msg = "已存在该账号";
                    return dataResult;
                }

                AdminEntity admin = new AdminEntity()
                {
                    account = adminEntity.account,
                    administrator = false,
                    createDate = DateTime.Now,
                    email = adminEntity.email ?? "",
                    forbidden = false,
                    modifyDate = DateTime.Now,
                    name = adminEntity.name,
                    password = Helper.DataEncrypt.DataMd5(adminEntity.password),
                    phone = adminEntity.phone ?? "",
                    userId  = adminEntity.userId
                };

                int rows = adminBLL.ActionDal.ActionDBAccess.Insertable(admin).ExecuteCommand();

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