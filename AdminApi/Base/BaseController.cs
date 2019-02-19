using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdminApi.Base
{
    /// <summary>
    /// 基础控制器
    /// </summary>
    public class BaseController : ControllerBase
    {
        /// <summary>
        /// 根据token获取个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected AdminEntity GetAdminByToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                AdminBLL adminBLL = new AdminBLL();
                AdminTokenBLL adminTokenBLL = new AdminTokenBLL();
                AdminTokenEntity adminTokenEntity = adminTokenBLL.GetByToken(token);

                AdminEntity adminEntity = adminBLL.GetById(adminTokenEntity.adminId);

                return adminEntity;
            }
            else
            {
                return null;
            }

        }
    }
}