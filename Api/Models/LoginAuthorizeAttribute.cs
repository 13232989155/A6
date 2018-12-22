using BLL;
using Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.Models
{
    public class LoginAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //获取跳过验证的标签如果有则跳过验证
            var isDefined = false;
            var controllerActionDescriptor = filterContext.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null)
            {
                isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true)
                  .Any(a => a.GetType().Equals(typeof(SkipCheckLoginAttribute)));
            }

            if ( isDefined)
            {
                return;
            }

            DataResult dr = new DataResult()
            {
                code = "300",
                msg = "参数是必需的"
            };
            //dynamic rpas = filterContext.ActionArguments.First().Value as dynamic;

            var para = filterContext.HttpContext.Request.Form; //rpas["token"];
            //if (rpas == null)
            //{

            //    filterContext.Result = new JsonResult(dr);
            //    return;
            //}
            string token = para["token"];

            if (string.IsNullOrWhiteSpace( token))
            {
                dr.msg = "token为空!";
                filterContext.Result = new JsonResult(dr);
                return;
            }

            UserTokenBLL userTokenBLL = new UserTokenBLL();
            UserTokenEntity userTokenEntity = userTokenBLL.GetByToken(token);

            if (userTokenEntity == null)
            {
                dr.msg = "token错误!";
                filterContext.Result = new JsonResult(dr);
                return;
            }

            int difference = 10000;
            if (DateTime.Now.Subtract(userTokenEntity.createDate).Minutes > difference)
            {
                dr.msg = "token过期!";
                filterContext.Result = new JsonResult(dr);
                return;
            }

            userTokenBLL.UpdateTime(userTokenEntity);

            base.OnActionExecuting(filterContext);
        }
    }

    
}