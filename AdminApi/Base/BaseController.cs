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
    public class BaseController : Controller
    {
        /// <summary>
        /// 根据token获取个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected UserEntity GetUserByToken(string token)
        {
            if (!string.IsNullOrWhiteSpace(token))
            {
                UserBLL userBLL = new UserBLL();
                UserTokenBLL userTokenBLL = new UserTokenBLL();
                UserTokenEntity userTokenEntity = userTokenBLL.GetByToken(token);

                UserEntity userEntity = userBLL.GetById(userTokenEntity.userId);

                return userEntity;
            }
            else
            {
                return null;
            }

        }
    }
}