using BLL;
using Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Api.Base
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseController : Controller
    {

        /// <summary>
        /// 根据token获取个人信息
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected UserEntity GetUserByToken(string token)
        {
            if ( !string.IsNullOrWhiteSpace(token))
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
