using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdminApi.Base;
using AdminApi.Models;
using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Controllers
{
    /// <summary>
    /// 管理员
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : BaseController
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="account">*</param>
        /// <param name="password">*</param>
        /// <returns></returns>
        [HttpPost]
        [SkipCheckLogin]
        public ActionResult<DataResult> Login(string account, string password)
        {
            DataResult dataResult = new DataResult();

            if (string.IsNullOrWhiteSpace(account) || string.IsNullOrWhiteSpace(password))
            {
                dataResult.msg = "账号和密码不能为空";
                dataResult.code = "201";
                return dataResult;
            }
            AdminBLL adminBLL = new AdminBLL();
            AdminEntity adminEntity = adminBLL.GetAccountAndPassword(account, Helper.DataEncrypt.DataMd5(password));

            if (adminEntity == null)
            {
                dataResult.msg = "账号或密码错误";
                dataResult.code = "201";
                return dataResult;
            }

            if (adminEntity.forbidden)
            {
                dataResult.msg = "账号已被禁用";
                dataResult.code = "201";
                return dataResult;
            }

            LoginResult loginResult = new LoginResult();
            loginResult.adminEntity = adminEntity;

            AdminTokenBLL adminTokenBLL = new AdminTokenBLL();
            AdminTokenEntity adminTokenEntity = adminTokenBLL.GetByAdminId(adminEntity.adminId);
            AdminTokenEntity adminToken = new AdminTokenEntity();

            if (adminTokenEntity == null)
            {
                adminToken = adminTokenBLL.Create(adminEntity.adminId);
            }
            else
            {
                adminToken = adminTokenBLL.Update(adminTokenEntity);
            }

            loginResult.token = adminToken.token;

            dataResult.data = loginResult;
            dataResult.code = "200";

            return dataResult;
        }

        /// <summary>
        /// 根据id获取信息
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="adminId">*</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> GetById(string token, int adminId)
        {
            DataResult dataResult = new DataResult();

            if (adminId < 10000)
            {
                dataResult.msg = "参数错误";
                dataResult.code = "201";
                return dataResult;
            }

            AdminBLL adminBLL = new AdminBLL();
            dataResult.data = adminBLL.GetById(adminId);
            dataResult.code = "200";
            return dataResult;
        }

        /// <summary>
        /// 获取自己信息
        /// </summary>
        /// <param name="token">*</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Get(string token)
        {
            DataResult dataResult = new DataResult();

            if (string.IsNullOrWhiteSpace(token))
            {
                dataResult.msg = "参数错误";
                dataResult.code = "201";
                return dataResult;
            }

            AdminBLL adminBLL = new AdminBLL();
            dataResult.data = GetAdminByToken(token);
            dataResult.code = "200";
            return dataResult;
        }

        /// <summary>
        /// 创建管理员
        /// </summary>
        /// <param name="token">*</param>
        /// <param name="adminEntity">*</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<DataResult> Crete(string token, [FromForm] AdminEntity adminEntity)
        {
            DataResult dataResult = new DataResult();
            return dataResult;
        }

    }
}