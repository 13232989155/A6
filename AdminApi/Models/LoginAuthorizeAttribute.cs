using BLL;
using Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AdminApi.Models
{
    /// <summary>
    /// 过滤器
    /// </summary>
    public class LoginAuthorizeAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 重写，登录过滤
        /// </summary>
        /// <param name="filterContext"></param>
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

            if (isDefined)
            {
                return;
            }

            DataResult dr = new DataResult();
            if (filterContext.ActionArguments.Count < 1)
            {
                dr.code = "300";
                dr.msg = "参数是必需的";
                filterContext.Result = new JsonResult(dr);
                return;
            }

            dynamic pars = filterContext.ActionArguments;

            if (!pars.ContainsKey("token"))
            {
                dr.code = "300";
                dr.msg = "未包含token";
                filterContext.Result = new JsonResult(dr);
                return;
            }

            // 获取token
            string token = pars["token"];

            if (string.IsNullOrWhiteSpace(token))
            {
                dr.msg = "token为空!";
                filterContext.Result = new JsonResult(dr);
                return;
            }

            AdminTokenBLL adminTokenBLL = new AdminTokenBLL();
            AdminTokenEntity adminTokenEntity = adminTokenBLL.GetByToken(token);

            if (adminTokenEntity == null)
            {
                dr.msg = "token错误!";
                filterContext.Result = new JsonResult(dr);
                return;
            }

            int difference = 10000;
            if (DateTime.Now.Subtract(adminTokenEntity.createDate).Minutes > difference)
            {
                dr.msg = "token过期!";
                filterContext.Result = new JsonResult(dr);
                return;
            }

            adminTokenBLL.UpdateTime(adminTokenEntity);

            base.OnActionExecuting(filterContext);
        }

        
    }

}
